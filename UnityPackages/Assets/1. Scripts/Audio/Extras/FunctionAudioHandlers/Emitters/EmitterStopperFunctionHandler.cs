using Audio.Core;
using Enums;
using Enums.Audio;
using UnityEngine;

namespace Audio.Extras.FunctionAudioHandlers.Emitters
{
	/// <summary>
	/// Stop the emitter as a reaction to a given unity event function
	/// </summary>
	public class EmitterStopperFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private EmitterType emitterType;
		
		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			AudioPlayer.StopEmitter(emitterType);
		}
	}
}