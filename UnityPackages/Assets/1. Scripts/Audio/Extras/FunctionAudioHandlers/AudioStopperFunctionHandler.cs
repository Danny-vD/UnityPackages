using Audio.Audioplayers.AudioFunctionHandlers;
using Enums;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio.Extras.FunctionAudioHandlers
{
	/// <summary>
	/// Stops the <see cref="AudioPlayerFunctionHandler"/> on a specific <see cref="Enums.UnityFunction"/>
	/// </summary>
	public class AudioStopperFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private AudioPlayerFunctionHandler audioPlayerFunctionHandler;
		
		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			EventInstance eventInstance = audioPlayerFunctionHandler.GetInstance;

			eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}

		private void Reset()
		{
			audioPlayerFunctionHandler = GetComponent<AudioPlayerFunctionHandler>();
		}
	}
}