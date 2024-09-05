using System.Collections.Generic;
using FMOD.Studio;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;

namespace FMODUtilityPackage.Audioplayers
{
	/// <summary>
	/// A manager class responsible for taking care of the 'global' event instances (the EventInstances that are used by multiple classes)
	/// </summary>
	public static class GlobalEventInstanceManager
	{
		private static Dictionary<AudioEventType, EventInstance> globalEventInstances;

		public static EventInstance CacheNewInstanceIfNeeded(AudioEventType audioEventType)
		{
			EventInstance eventInstance;

			eventInstance = !globalEventInstances.ContainsKey(audioEventType) ? CacheNewInstance(audioEventType) : GetEventInstance(audioEventType);

			return eventInstance;
		}

		public static EventInstance GetEventInstance(AudioEventType audioEventType)
		{
			if (globalEventInstances.TryGetValue(audioEventType, out EventInstance eventInstance))
			{
				return eventInstance;
			}

			return CacheNewInstance(audioEventType);
		}

		public static void StopAllInstances(STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
		{
			foreach (KeyValuePair<AudioEventType, EventInstance> pair in globalEventInstances)
			{
				EventInstance eventInstance = pair.Value;
				
				eventInstance.release();
				eventInstance.stop(stopMode);
			}
			
			globalEventInstances.Clear();
		}

		public static void FreeAndRemoveInstance(AudioEventType audioEventType, bool stopInstance, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
		{
			if (globalEventInstances.TryGetValue(audioEventType, out EventInstance eventInstance))
			{
				eventInstance.release();

				if (stopInstance)
				{
					eventInstance.stop(stopMode);
				}

				globalEventInstances.Remove(audioEventType);
			}
		}

		private static EventInstance CacheNewInstance(AudioEventType audioEventType)
		{
			EventInstance eventInstance = AudioPlayer.GetEventInstance(audioEventType);

			globalEventInstances.Add(audioEventType, eventInstance);

			return eventInstance;
		}
	}
}