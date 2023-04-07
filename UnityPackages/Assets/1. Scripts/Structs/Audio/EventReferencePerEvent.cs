using System;
using FMODUnity;
using VDFramework.Interfaces;
using UnityEngine;
using EventType = Enums.Audio.EventType;

namespace Structs.Audio
{
	[Serializable]
	public struct EventReferencePerEvent : IKeyValuePair<EventType, EventReference>
	{
		[SerializeField]
		private EventType key;

		[SerializeField]
		private EventReference value;

		public EventType Key
		{
			get => key;
			set => key = value;
		}

		public EventReference Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<EventType, EventReference> other)
		{
			return other != null && other.Key == Key;
		}
	}
}