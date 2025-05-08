using FMODUnity;
using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Structs;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.EmitterHandlers
{
	/// <summary>
	/// Set parameters to the emitter as a reaction to given unity event functions
	/// </summary>
	public class EmitterParametersFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private GlobalEmitter globalEmitter;
		
		[SerializeField]
		private SerializableEnumDictionary<UnityFunction, EventParameters> parameters;

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			StudioEventEmitter emitter = AudioManager.Instance.FMODPathResolver.GetEmitter(globalEmitter);

			emitter.SetParameters(parameters[unityFunction]);
		}
	}
}