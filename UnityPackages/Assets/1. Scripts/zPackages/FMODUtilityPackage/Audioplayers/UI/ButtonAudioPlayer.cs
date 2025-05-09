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
	/// <summary>
	/// A simple script that plays an <see cref="AudioEvent"/> when the button is clicked
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class ButtonAudioPlayer : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField, Tooltip("If true, clicking the button again will start the event from the beginning")]
		private bool clickRestartsAudio = true;

		[SerializeField]
		private AudioEvent audioEventToPlayOnClick;

		[SerializeField]
		private EventParameters parameters;

		[Header("Global Instance settings")]
		[SerializeField]
		private bool useGlobalInstance;

		[SerializeField]
		private bool releaseGlobalInstanceOnDisable;

		private Button button;

		private EventInstance localClickAudioEvent;
		private bool isInitialized;

		private EventInstance AudioEventInstance => useGlobalInstance ? GlobalEventInstanceManager.GetEventInstance(audioEventToPlayOnClick) : localClickAudioEvent;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(clickRestartsAudio ? Play : PlayIfNotPlaying);
		}

		private void Initialize()
		{
			EventInstance audioEventToPlayOnClickInstance;

			if (!useGlobalInstance)
			{
				localClickAudioEvent            = AudioPlayer.GetEventInstance(audioEventToPlayOnClick);
				audioEventToPlayOnClickInstance = localClickAudioEvent;
			}
			else
			{
				audioEventToPlayOnClickInstance = GlobalEventInstanceManager.GetEventInstance(audioEventToPlayOnClick);
			}

			isInitialized = true;

			audioEventToPlayOnClickInstance.SetParameters(parameters);
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
				localClickAudioEvent.release();
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

			EventInstance clickAudioEventInstance = AudioEventInstance;

			clickAudioEventInstance.getPlaybackState(out PLAYBACK_STATE state);

			if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
			{
				clickAudioEventInstance.start();
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

			button.onClick.RemoveListener(clickRestartsAudio ? Play : PlayIfNotPlaying);
		}
	}
}