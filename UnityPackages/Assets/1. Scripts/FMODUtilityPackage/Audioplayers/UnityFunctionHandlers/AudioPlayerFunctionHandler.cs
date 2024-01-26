using Enums;
using FMOD.Studio;
using FMODUtilityPackage.Audioplayers.UnityFunctionHandlers.BaseClasses;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using UnityEngine;
using EventType = FMODUtilityPackage.Enums.EventType;

namespace FMODUtilityPackage.Audioplayers.UnityFunctionHandlers
{
	/// <summary>
	/// play an <see cref="EventType"/> on a specific <see cref="Enums.UnityFunction"/>
	/// </summary>
	public class AudioPlayerFunctionHandler : AbstractAudioFunctionHandler
	{
		[SerializeField]
		private EventType eventType;
		
		[Header("On Destroy"), SerializeField, Tooltip("Stop all instances when this object is destroyed")]
		private bool stopInstancesOnDestroy;
		
		[SerializeField, Tooltip("Allow the playing events to fade out when this object is destroyed")]
		private bool allowFadeoutOnDestroy = true;

		private EventInstance eventInstance;

		public EventInstance AudioEventInstance => eventInstance;

		private void Awake()
		{
			eventInstance = AudioPlayer.GetEventInstance(eventType);
		}

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			eventInstance.start();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (stopInstancesOnDestroy)
			{
				STOP_MODE stopMode = allowFadeoutOnDestroy ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

				eventInstance.stop(stopMode);
			}
			
			eventInstance.release();
		}
	}
}