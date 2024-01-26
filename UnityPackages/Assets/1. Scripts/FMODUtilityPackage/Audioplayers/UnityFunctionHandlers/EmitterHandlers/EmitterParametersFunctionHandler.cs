using Enums;
using FMODUnity;
using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Structs;
using UnityEngine;
using Utility.SerializableDictionary;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.EmitterHandlers
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