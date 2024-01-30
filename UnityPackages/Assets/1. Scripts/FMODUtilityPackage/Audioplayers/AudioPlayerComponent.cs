using FMOD.Studio;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Interfaces;
using FMODUtilityPackage.Structs;
using UnityEngine;
using VDFramework;

namespace FMODUtilityPackage.Audioplayers
{
	public class AudioPlayerComponent : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField]
		private AudioEventType audioEventType;

		private EventInstance eventInstance;

		private void Start()
		{
			CacheEventInstance();
		}

		public void SetEventType(AudioEventType newAudioEventType)
		{
			audioEventType = newAudioEventType;
			CacheEventInstance();
		}
		
		public void Play()
		{
			eventInstance.start();
		}

		public void PlayIfNotPlaying()
		{
			eventInstance.getPlaybackState(out PLAYBACK_STATE state);

			if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
			{
				eventInstance.start();
			}
		}

		public void SetPause(bool paused)
		{
			eventInstance.setPaused(paused);
		}

		public void Stop()
		{
			Stop(STOP_MODE.ALLOWFADEOUT);
		}
		
		public void Stop(STOP_MODE stopMode)
		{
			eventInstance.stop(stopMode);
		}

		public void SetParameters(EventParameters parameters)
		{
			eventInstance.SetParameters(parameters);
		}

		private void CacheEventInstance()
		{
			eventInstance.release();
			eventInstance = AudioPlayer.GetEventInstance(audioEventType);
		}

		private void OnDestroy()
		{
			eventInstance.release();
		}
	}
}