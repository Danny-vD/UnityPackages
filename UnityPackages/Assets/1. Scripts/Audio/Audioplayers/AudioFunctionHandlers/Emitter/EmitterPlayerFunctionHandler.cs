using Audio.Core;
using Audio.Extras;
using Enums;
using Enums.Audio;
using UnityEngine;

namespace Audio.Audioplayers.AudioFunctionHandlers.Emitter
{
	/// <summary>
	/// Play the emitter as a reaction to a given unity event function
	/// </summary>
	public class EmitterPlayerFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private EmitterType emitterType;
		
		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			AudioPlayer.PlayEmitter(emitterType);
		}
	}
}