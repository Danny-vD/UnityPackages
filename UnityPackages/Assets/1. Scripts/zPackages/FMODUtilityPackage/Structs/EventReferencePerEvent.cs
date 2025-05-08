using System;
using FMODUnity;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;

namespace FMODUtilityPackage.Structs
{
	[Serializable]
	public struct EventReferencePerEvent : IKeyValuePair<AudioEvent, EventReference>
	{
		[SerializeField]
		private AudioEvent key;

		[SerializeField]
		private EventReference value;

		public AudioEvent Key
		{
			get => key;
			set => key = value;
		}

		public EventReference Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<AudioEvent, EventReference> other)
		{
			return other != null && other.Key == Key;
		}
	}
}