using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.Managers;
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

		[SerializeField, Tooltip("Don't restart the instance if it is already playing")]
		private bool onlyPlayIfNotPlaying = false;
		
		[Header("On Destroy"), SerializeField, Tooltip("Stop playing the event when this object is destroyed")]
		private bool stopPlayingOnDestroy;

		[SerializeField, Tooltip("Allow the playing events to fade out when this object is destroyed")]
		private bool allowFadeoutOnDestroy = true;

		[Header("Global Instance settings")]
		[SerializeField, Tooltip("Free the memory of the global instance when this object is destroyed")]
		private bool freeGlobalInstanceOnDestroy = true;

		private EventInstance localInstance;

		public EventInstance AudioEventInstance => useGlobalInstance ? GlobalEventInstanceManager.GetEventInstance(audioEventType) : localInstance;

		private void Awake()
		{
			if (useGlobalInstance)
			{
				GlobalEventInstanceManager.CacheNewInstanceIfNeeded(audioEventType);
			}
			else
			{
				localInstance = AudioPlayer.GetEventInstance(audioEventType);
			}
		}

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			EventInstance audioEventInstance = AudioEventInstance;
			
			if (onlyPlayIfNotPlaying)
			{
				audioEventInstance.getPlaybackState(out PLAYBACK_STATE state);

				if (state is PLAYBACK_STATE.PLAYING or PLAYBACK_STATE.STARTING)
				{
					return;
				}
			}

			audioEventInstance.start();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			STOP_MODE stopMode = allowFadeoutOnDestroy ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

			if (useGlobalInstance)
			{
				if (freeGlobalInstanceOnDestroy)
				{
					GlobalEventInstanceManager.ReleaseAndRemoveInstance(audioEventType, stopPlayingOnDestroy, stopMode);
				}
				else if (stopPlayingOnDestroy)
				{
					AudioEventInstance.stop(stopMode);
				}
			}
			else
			{
				if (stopPlayingOnDestroy)
				{
					localInstance.stop(stopMode); // Using localInstance here to prevent another UseGlobalInstance check 
				}

				localInstance.release();
			}
		}
	}
}