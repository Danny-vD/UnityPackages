using FMOD.Studio;
using UnityEngine;
using VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using VDPackages.UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.AudioPlayerFunctionHandlerExtras
{
	/// <summary>
	/// Stops the <see cref="AudioPlayerFunctionHandler"/> on a specific <see cref="UnityFunction"/>
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