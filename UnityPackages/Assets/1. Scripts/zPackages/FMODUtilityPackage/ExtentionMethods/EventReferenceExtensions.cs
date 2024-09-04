using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

namespace FMODUtilityPackage.ExtentionMethods
{
	public static class EventReferenceExtensions
	{
		public static EventDescription GetEventDescription(this ref EventReference instance)
		{
			return RuntimeManager.GetEventDescription(instance.Guid);
		}

		public static PARAMETER_DESCRIPTION GetParameterDescriptionByName(this EventReference instance, string parameterName)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(instance.Guid);
			eventDescription.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);

			return parameterDescription;
		}

		public static IEnumerable<PARAMETER_DESCRIPTION> GetParameters(this EventReference instance)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(instance.Guid);
			eventDescription.getParameterDescriptionCount(out int count);

			PARAMETER_DESCRIPTION[] array = new PARAMETER_DESCRIPTION[count];
			
			for (int i = 0; i < count; i++)
			{
				eventDescription.getParameterDescriptionByIndex(i, out PARAMETER_DESCRIPTION parameterDescription);
				array[i] = parameterDescription;
			}

			return array;
		}
	}
}