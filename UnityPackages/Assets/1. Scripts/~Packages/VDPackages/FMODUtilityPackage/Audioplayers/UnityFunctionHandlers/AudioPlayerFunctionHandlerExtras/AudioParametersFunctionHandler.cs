using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using VDPackages.FMODUtilityPackage.Structs;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.AudioPlayerFunctionHandlerExtras
{
	/// <summary>
	/// Sets parameters to the <see cref="AudioPlayerFunctionHandler"/> on given <see cref="UnityFunction"/>s
	/// </summary>
	public class AudioParametersFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private AudioPlayerFunctionHandler audioPlayerFunctionHandler;
		
		[SerializeField]
		private SerializableEnumDictionary<UnityFunction, EventParameters> parameters;

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			EventInstance eventInstance = audioPlayerFunctionHandler.AudioEventInstance;

			foreach (KeyValuePair<string, float> pair in parameters[unityFunction])
			{
				eventInstance.setParameterByName(pair.Key, pair.Value);
			}
		}

		private void Reset()
		{
			audioPlayerFunctionHandler = GetComponent<AudioPlayerFunctionHandler>();
		}
	}
}