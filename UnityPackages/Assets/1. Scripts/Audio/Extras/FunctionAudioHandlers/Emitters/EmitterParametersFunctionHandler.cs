using Audio.Core;
using Enums;
using Enums.Audio;
using ExtentionMethods.FMODUnity;
using FMODUnity;
using Structs.Audio;
using UnityEngine;
using Utility.SerializableDictionary;

namespace Audio.Extras.FunctionAudioHandlers.Emitters
{
	/// <summary>
	/// Set parameters to the emitter as a reaction to given unity event functions
	/// </summary>
	public class EmitterParametersFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private EmitterType emitterType;
		
		[SerializeField]
		private SerializableEnumDictionary<UnityFunction, EventParameters> parameters;

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			StudioEventEmitter emitter = AudioManager.Instance.EventPaths.GetEmitter(emitterType);

			emitter.SetParameters(parameters[unityFunction]);
		}
	}
}