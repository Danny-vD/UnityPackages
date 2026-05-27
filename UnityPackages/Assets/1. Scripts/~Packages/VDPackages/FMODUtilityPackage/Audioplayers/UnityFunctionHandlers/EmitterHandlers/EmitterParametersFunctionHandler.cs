using FMODUnity;
using UnityEngine;
using VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using VDPackages.FMODUtilityPackage.Core;
using VDPackages.FMODUtilityPackage.Enums;
using VDPackages.FMODUtilityPackage.ExtentionMethods;
using VDPackages.FMODUtilityPackage.Structs;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.EmitterHandlers
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