using FMOD.Studio;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Interfaces;
using UnityEngine;
using VDFramework;
using EventType = FMODUtilityPackage.Enums.EventType;

namespace FMODUtilityPackage.Audioplayers
{
	public class AudioEventPlayer : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField]
		private EventType eventType;

		private EventInstance eventInstance;

		private void Start()
		{
			CacheEventInstance();
		}

		public void SetEventType(EventType newEventType)
		{
			eventType = newEventType;
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

		private void CacheEventInstance()
		{
			eventInstance.release();
			eventInstance = AudioPlayer.GetEventInstance(eventType);
		}

		private void OnDestroy()
		{
			eventInstance.release();
		}
	}
}