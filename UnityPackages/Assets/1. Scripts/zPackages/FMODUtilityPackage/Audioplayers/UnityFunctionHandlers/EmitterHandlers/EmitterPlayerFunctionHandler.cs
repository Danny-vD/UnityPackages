using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.EmitterHandlers
{
	/// <summary>
	/// Play the emitter as a reaction to a given unity event function
	/// </summary>
	public class EmitterPlayerFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private GlobalEmitter globalEmitter;
		
		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			AudioPlayer.PlayEmitter(globalEmitter);
		}
	}
}