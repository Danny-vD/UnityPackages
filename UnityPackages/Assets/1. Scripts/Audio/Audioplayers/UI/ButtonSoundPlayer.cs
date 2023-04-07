using Audio.Core;
using ExtentionMethods.FMODUnity;
using FMOD.Studio;
using Interfaces;
using Interfaces.Audio;
using Structs.Audio;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using EventType = Enums.Audio.EventType;

namespace Audio.Audioplayers.UI
{
	[RequireComponent(typeof(Button))]
	public class ButtonSoundPlayer : BetterMonoBehaviour, IAudioplayer
	{
		[SerializeField, Tooltip("If true, clicking the button again will start the event from the beginning")]
		private bool clickRestartsSound = true;

		[SerializeField]
		private EventType eventToPlayOnClick;

		[SerializeField]
		private EventParameters parameters;

		private Button button;

		private EventInstance clickSound;
		private bool isInitialized;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(clickRestartsSound ? Play : PlayIfNotPlaying);
		}

		private void Initialize()
		{
			clickSound = AudioPlayer.GetEventInstance(eventToPlayOnClick);
			clickSound.SetParameters(parameters);

			isInitialized = true;
		}

		private void OnDisable()
		{
			clickSound.release();
		}

		private void OnDestroy()
		{
			Stop();
		}

		public void Play()
		{
			if (!isInitialized)
			{
				Initialize();
			}

			clickSound.start();
		}

		public void PlayIfNotPlaying()
		{
			if (!isInitialized)
			{
				Initialize();
			}

			clickSound.getPlaybackState(out PLAYBACK_STATE state);

			if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
			{
				clickSound.start();
			}
		}

		public void Stop()
		{
			clickSound.stop(STOP_MODE.ALLOWFADEOUT);
		}

		public void SetPause(bool paused)
		{
			clickSound.setPaused(paused);
		}

		public void SetParameters(EventParameters eventParameters)
		{
			parameters = eventParameters;
			clickSound.SetParameters(parameters);
		}
	}
}