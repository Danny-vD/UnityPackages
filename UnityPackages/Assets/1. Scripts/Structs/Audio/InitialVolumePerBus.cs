using System;
using Enums.Audio;
using UnityEngine;
using VDFramework.Interfaces;

namespace Structs.Audio
{
	[Serializable]
	public struct InitialVolumePerBus : IKeyValuePair<BusType, float>
	{
		[SerializeField]
		private BusType key;

		[SerializeField]
		private float value;

		public bool isMuted;

		public static InitialVolumePerBus DefaultValue => new InitialVolumePerBus(default, 1, false);

		public BusType Key
		{
			get => key;
			set => key = value;
		}

		public float Value
		{
			get => value;
			set => this.value = value;
		}

		public InitialVolumePerBus(BusType busType, float volumeValue, bool muted)
		{
			key     = busType;
			value   = volumeValue;
			isMuted = muted;
		}

		public bool Equals(IKeyValuePair<BusType, float> other)
		{
			return other != null && other.Key == Key;
		}
	}
}