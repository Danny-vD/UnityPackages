using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using UnityEngine;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.AudioPlayerFunctionHandlerExtras
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
			EventInstance eventInstance = audioPlayerFunctionHandler.AudioEventInstance;

			eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}

		private void Reset()
		{
			audioPlayerFunctionHandler = GetComponent<AudioPlayerFunctionHandler>();
		}
	}
}