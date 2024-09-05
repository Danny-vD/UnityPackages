using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using UnityEngine;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers
{
	/// <summary>
	/// play an <see cref="AudioEventType"/> on a specific <see cref="UnityFunction"/>
	/// </summary>
	public class AudioPlayerFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private AudioEventType audioEventType;

		[Header("Playback settings"), SerializeField, Tooltip("Share this event instance between all AudioPlayerFunctionHandlers")]
		private bool useGlobalInstance = false;

		[SerializeField, Tooltip("Free the memory of the global instance when this object is destroyed")]
		private bool freeGlobalInstanceOnDestroy = true;
		
		[SerializeField, Tooltip("Stop playing when this object is destroyed")]
		private bool stopGlobalInstanceOnDestroy = true;

		[Header("On Destroy"), SerializeField, Tooltip("Stop all instances when this object is destroyed")]
		private bool stopInstancesOnDestroy;

		[SerializeField, Tooltip("Allow the playing events to fade out when this object is destroyed")]
		private bool allowFadeoutOnDestroy = true;

		private EventInstance eventInstance;

		public EventInstance AudioEventInstance => eventInstance;

		private void Awake()
		{
			if (useGlobalInstance)
			{
				eventInstance = GlobalEventInstanceManager.CacheNewInstanceIfNeeded(audioEventType);
			}
			else
			{
				eventInstance = AudioPlayer.GetEventInstance(audioEventType);
			}
		}

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			eventInstance.start();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			STOP_MODE stopMode = allowFadeoutOnDestroy ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;
			
			if (useGlobalInstance)
			{
				if (freeGlobalInstanceOnDestroy)
				{
					GlobalEventInstanceManager.FreeAndRemoveInstance(audioEventType, stopInstancesOnDestroy, stopMode);
				}
				else if (stopGlobalInstanceOnDestroy)
				{
					eventInstance.stop(stopMode);
				}
			}
			else
			{
				if (stopInstancesOnDestroy)
				{
					eventInstance.stop(stopMode);
				}

				eventInstance.release();
			}
		}
	}
}