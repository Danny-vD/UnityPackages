using System.Collections.Generic;
using FMOD.Studio;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;

namespace FMODUtilityPackage.Audioplayers.Managers
{
	/// <summary>
	/// A manager class responsible for taking care of the 'global' event instances (the EventInstances that are used by multiple classes)
	/// </summary>
	public static class GlobalEventInstanceManager
	{
		private static readonly Dictionary<AudioEvent, EventInstance> globalEventInstances = new Dictionary<AudioEvent, EventInstance>();

		/// <summary>
		/// Tell the <see cref="GlobalEventInstanceManager"/> that this AudioEvent will likely be used and should therefore be cached in advance
		/// </summary>
		public static EventInstance CacheNewInstanceIfNeeded(AudioEvent audioEvent) // Used as an 'announcement' that something will be used in the future
		{
			EventInstance eventInstance = !globalEventInstances.ContainsKey(audioEvent) ? CacheNewInstance(audioEvent) : GetEventInstance(audioEvent);

			return eventInstance;
		}

		public static bool HasInstanceOfEvent(AudioEvent audioEvent)
		{
			return globalEventInstances.ContainsKey(audioEvent);
		}

		public static bool TryGetEventInstance(AudioEvent audioEvent, out EventInstance eventInstance)
		{
			return globalEventInstances.TryGetValue(audioEvent, out eventInstance);
		}

		/// <summary>
		/// Will return the globally accessible instance for this <see cref="AudioEvent"/> or create a new one if one is not cached
		/// </summary>
		public static EventInstance GetEventInstance(AudioEvent audioEvent)
		{
			if (globalEventInstances.TryGetValue(audioEvent, out EventInstance eventInstance))
			{
				return eventInstance;
			}

			return CacheNewInstance(audioEvent);
		}
		
		public static void StopAllInstances(STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
		{
			foreach (KeyValuePair<AudioEvent, EventInstance> pair in globalEventInstances)
			{
				EventInstance eventInstance = pair.Value;

				eventInstance.stop(stopMode);
				eventInstance.release();
			}

			globalEventInstances.Clear();
		}

		public static void ReleaseAndRemoveInstance(AudioEvent audioEvent, bool stopInstance, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
		{
			if (globalEventInstances.TryGetValue(audioEvent, out EventInstance eventInstance))
			{
				if (stopInstance)
				{
					eventInstance.stop(stopMode);
				}

				eventInstance.release();

				globalEventInstances.Remove(audioEvent);
			}
		}

		private static EventInstance CacheNewInstance(AudioEvent audioEvent)
		{
			EventInstance eventInstance = AudioPlayer.GetEventInstance(audioEvent);

			globalEventInstances.Add(audioEvent, eventInstance);

			return eventInstance;
		}
	}
}