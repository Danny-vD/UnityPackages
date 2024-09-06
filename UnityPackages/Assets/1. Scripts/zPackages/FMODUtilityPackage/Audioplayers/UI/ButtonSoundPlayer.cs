using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.Managers;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Interfaces;
using FMODUtilityPackage.Structs;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;

namespace FMODUtilityPackage.Audioplayers.UI
{
	[RequireComponent(typeof(Button))]
	public class ButtonSoundPlayer : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField, Tooltip("If true, clicking the button again will start the event from the beginning")]
		private bool clickRestartsSound = true;

		[SerializeField]
		private AudioEventType audioEventToPlayOnClick;

		[SerializeField]
		private EventParameters parameters;

		[Header("Global Instance settings")]
		[SerializeField]
		private bool useGlobalInstance;

		[SerializeField]
		private bool releaseGlobalInstanceOnDisable;

		private Button button;

		private EventInstance localClickSoundEvent;
		private bool isInitialized;

		private EventInstance AudioEventInstance => useGlobalInstance ? GlobalEventInstanceManager.GetEventInstance(audioEventToPlayOnClick) : localClickSoundEvent;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(clickRestartsSound ? Play : PlayIfNotPlaying);
		}

		private void Initialize()
		{
			EventInstance clickSoundEventInstance;
			
			if (!useGlobalInstance)
			{
				localClickSoundEvent    = AudioPlayer.GetEventInstance(audioEventToPlayOnClick);
				clickSoundEventInstance = localClickSoundEvent;
			}
			else
			{
				clickSoundEventInstance = GlobalEventInstanceManager.GetEventInstance(audioEventToPlayOnClick);
			}
			
			isInitialized = true;
			
			clickSoundEventInstance.SetParameters(parameters);
		}

		private void OnDisable()
		{
			if (useGlobalInstance)
			{
				if (releaseGlobalInstanceOnDisable)
				{
					GlobalEventInstanceManager.ReleaseAndRemoveInstance(audioEventToPlayOnClick, false);
				}
			}
			else
			{
				localClickSoundEvent.release();
			}
			
			isInitialized = false;
		}

		public void Play()
		{
			if (!isInitialized)
			{
				Initialize();
			}

			AudioEventInstance.start();
		}

		public void PlayIfNotPlaying()
		{
			if (!isInitialized)
			{
				Initialize();
			}

			EventInstance clickSoundEventInstance = AudioEventInstance;
			
			clickSoundEventInstance.getPlaybackState(out PLAYBACK_STATE state);

			if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
			{
				clickSoundEventInstance.start();
			}
		}

		public void Stop()
		{
			AudioEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}

		public void SetPause(bool paused)
		{
			AudioEventInstance.setPaused(paused);
		}

		public void SetParameters(EventParameters eventParameters)
		{
			parameters = eventParameters;
			AudioEventInstance.SetParameters(parameters);
		}

		private void OnDestroy()
		{
			Stop();

			button.onClick.RemoveListener(clickRestartsSound ? Play : PlayIfNotPlaying);
		}
	}
}