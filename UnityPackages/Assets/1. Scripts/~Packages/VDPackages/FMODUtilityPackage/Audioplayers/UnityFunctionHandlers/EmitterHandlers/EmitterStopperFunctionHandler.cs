using UnityEngine;
using VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using VDPackages.FMODUtilityPackage.Core;
using VDPackages.FMODUtilityPackage.Enums;
using VDPackages.UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace VDPackages.FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.EmitterHandlers
{
	/// <summary>
	/// Stop the emitter as a reaction to a given unity event function
	/// </summary>
	public class EmitterStopperFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private GlobalEmitter globalEmitter;
		
		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			AudioPlayer.StopEmitter(globalEmitter);
		}
	}
}