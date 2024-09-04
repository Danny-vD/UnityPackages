using System;
using FMODUnity;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;

namespace FMODUtilityPackage.Structs
{
	[Serializable]
	public struct EventReferencePerEvent : IKeyValuePair<AudioEventType, EventReference>
	{
		[SerializeField]
		private AudioEventType key;

		[SerializeField]
		private EventReference value;

		public AudioEventType Key
		{
			get => key;
			set => key = value;
		}

		public EventReference Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<AudioEventType, EventReference> other)
		{
			return other != null && other.Key == Key;
		}
	}
}