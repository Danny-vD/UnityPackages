using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

namespace FMODUtilityPackage.Core
{
	public static class AudioParameterManager
	{
		private static readonly Dictionary<string, PARAMETER_ID> globalParameters = new Dictionary<string, PARAMETER_ID>();

		/////////////////////////////////////////////////
		//			Global parameters
		/////////////////////////////////////////////////

		/// <summary>
		/// Sets a global parameter by retrieving its ID from the eventDescription
		/// </summary>
		public static void SetGlobalParameter(EventReference eventReference, string parameter, float newValue)
		{
			RuntimeManager.StudioSystem.setParameterByID(GetGlobalParameterID(eventReference, parameter), newValue);
		}

		/// <summary>
		/// Sets a global parameter by name or ID if cached
		/// <para> Use <see cref="SetGlobalParameter(FMODUnity.EventReference, string, float)"/> to cache the ID</para>
		/// </summary>
		public static void SetGlobalParameter(string parameter, float newValue)
		{
			if (globalParameters.TryGetValue(parameter, out PARAMETER_ID id))
			{
				RuntimeManager.StudioSystem.setParameterByID(id, newValue);
				return;
			}

			RuntimeManager.StudioSystem.setParameterByName(parameter, newValue);
		}

		private static PARAMETER_ID GetGlobalParameterID(EventReference eventReference, string parameter)
		{
			if (globalParameters.TryGetValue(parameter, out PARAMETER_ID id))
			{
				return id;
			}

			return AddNewCachedParameter(eventReference, parameter);
		}

		private static PARAMETER_ID GetParameterIDInternal(EventReference eventReference, string parameter)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(eventReference.Guid);
			eventDescription.getParameterDescriptionByName(parameter, out PARAMETER_DESCRIPTION parameterDescription);
			return parameterDescription.id;
		}

		private static PARAMETER_ID AddNewCachedParameter(EventReference eventReference, string parameter)
		{
			PARAMETER_ID id = GetParameterIDInternal(eventReference, parameter);

			globalParameters.Add(parameter, id);
			return id;
		}

		/////////////////////////////////////////////////
		//			Local parameters
		/////////////////////////////////////////////////

		/// <summary>
		/// Will set the parameter to the newValue for **ALL** instances of this event
		/// </summary>
		public static void SetEventParameter(EventReference eventReference, string parameter, float newValue)
		{
			RuntimeManager.GetEventDescription(eventReference.Guid).getInstanceList(out EventInstance[] instances);

			foreach (EventInstance instance in instances)
			{
				instance.setParameterByName(parameter, newValue);
			}
		}

		/////////////////////////////////////////////////
		//			Bus parameters
		/////////////////////////////////////////////////
	}
}