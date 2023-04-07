using System.Collections.Generic;
using Audio.Audioplayers.AudioFunctionHandlers;
using Enums;
using FMOD.Studio;
using Structs.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.SerializableDictionary;

namespace Audio.Extras.FunctionAudioHandlers
{
	/// <summary>
	/// Sets parameters to the <see cref="AudioPlayerFunctionHandler"/> on given <see cref="Enums.UnityFunction"/>s
	/// </summary>
	public class AudioParametersFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private AudioPlayerFunctionHandler audioPlayerFunctionHandler;
		
		[SerializeField]
		private SerializableEnumDictionary<UnityFunction, EventParameters> parameters;

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			EventInstance eventInstance = audioPlayerFunctionHandler.GetInstance;

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