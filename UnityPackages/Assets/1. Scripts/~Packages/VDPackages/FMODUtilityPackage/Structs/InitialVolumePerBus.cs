using System;
using UnityEngine;
using VDFramework.Interfaces;
using VDPackages.FMODUtilityPackage.Enums;

namespace VDPackages.FMODUtilityPackage.Structs
{
	[Serializable]
	public struct InitialVolumePerBus : IKeyValuePair<AudioBus, float>
	{
		[SerializeField]
		private AudioBus key;

		[SerializeField]
		private float value;

		public bool isMuted;

		public static InitialVolumePerBus DefaultValue => new InitialVolumePerBus(default, 1, false);

		public AudioBus Key
		{
			get => key;
			set => key = value;
		}

		public float Value
		{
			get => value;
			set => this.value = value;
		}

		public InitialVolumePerBus(AudioBus audioBus, float volumeValue, bool muted)
		{
			key     = audioBus;
			value   = volumeValue;
			isMuted = muted;
		}

		public bool Equals(IKeyValuePair<AudioBus, float> other)
		{
			return other != null && other.Key == Key;
		}
	}
}