using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Structs.Audio;

namespace ExtentionMethods.FMODUnity
{
	public static class StudioEventEmitterExtensions
	{
		private static readonly Dictionary<string, GUID> guidsPerEvent = new Dictionary<string, GUID>();
		
		public static PARAMETER_ID GetParameterID(this StudioEventEmitter emitter, string parameterName)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(emitter.EventReference.Guid);
			eventDescription.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);
			return parameterDescription.id;
		}

		private static GUID GetGuid(string emitterEventPath)
		{
			if (guidsPerEvent.TryGetValue(emitterEventPath, out GUID guid))
			{
				return guid;
			}

			guid = RuntimeManager.PathToGUID(emitterEventPath);
			guidsPerEvent.Add(emitterEventPath, guid);
			
			return guid;
		}
		
		public static void SetParameters(this StudioEventEmitter instance, EventParameters parameters)
		{
			foreach (KeyValuePair<string, float> pair in parameters)
			{
				instance.SetParameter(pair.Key, pair.Value);
			}
		}
	}
}