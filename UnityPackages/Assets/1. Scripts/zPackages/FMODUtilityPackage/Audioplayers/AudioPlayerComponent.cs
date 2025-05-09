using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.Managers;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Interfaces;
using FMODUtilityPackage.Structs;
using UnityEngine;
using UnityEngine.Serialization;
using VDFramework;

namespace FMODUtilityPackage.Audioplayers
{
	/// <summary>
	/// A simple component that provides functions to Set, Start, Pause and Stop <see cref="AudioEvent"/>s, optionally with parameters
	/// </summary>
	/// <seealso cref="IAudioplayer"/>
	public class AudioPlayerComponent : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField]
		private AudioEvent audioEvent;

		[Header("Global Instance settings")]
		[SerializeField]
		private bool useGlobalInstance;

		[SerializeField]
		private bool releaseGlobalInstanceOnDestroy = true;

		private EventInstance localInstance;
		private EventInstance AudioEventInstance => useGlobalInstance ? GlobalEventInstanceManager.GetEventInstance(audioEvent) : localInstance;

		private void Start()
		{
			CacheEventInstance();
		}

		public void SetEventType(AudioEvent newAudioEvent, bool releaseGlobalInstanceIfApplicable)
		{
			if (useGlobalInstance && releaseGlobalInstanceIfApplicable)
			{
				GlobalEventInstanceManager.ReleaseAndRemoveInstance(audioEvent, false);
			}

			audioEvent = newAudioEvent;
			CacheEventInstance();
		}

		public void Play()
		{
			AudioEventInstance.start();
		}

		public void PlayIfNotPlaying()
		{
			EventInstance audioEventInstance = AudioEventInstance;
			audioEventInstance.getPlaybackState(out PLAYBACK_STATE state);

			if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
			{
				audioEventInstance.start();
			}
		}

		public void SetPause(bool paused)
		{
			AudioEventInstance.setPaused(paused);
		}

		public void Stop()
		{
			Stop(STOP_MODE.ALLOWFADEOUT);
		}

		public void Stop(STOP_MODE stopMode)
		{
			AudioEventInstance.stop(stopMode);
		}

		public void SetParameters(EventParameters parameters)
		{
			AudioEventInstance.SetParameters(parameters);
		}

		private void CacheEventInstance()
		{
			if (useGlobalInstance)
			{
				GlobalEventInstanceManager.CacheNewInstanceIfNeeded(audioEvent);
			}
			else
			{
				localInstance.release(); // No need to check for null because it is a struct
				localInstance = AudioPlayer.GetEventInstance(audioEvent);
			}
		}

		private void OnDestroy()
		{
			if (useGlobalInstance && releaseGlobalInstanceOnDestroy)
			{
				AudioEventInstance.release();
			}
			else
			{
				localInstance.release();
			}
		}
	}
}