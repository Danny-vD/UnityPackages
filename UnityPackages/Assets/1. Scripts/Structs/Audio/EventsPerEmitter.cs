using System;
using Enums.Audio;
using UnityEngine;
using VDFramework.Interfaces;
using EventType = Enums.Audio.EventType;

namespace Structs.Audio
{
	[Serializable]
	public struct EventsPerEmitter : IKeyValuePair<EmitterType, EventType>
	{
		[SerializeField]
		private EmitterType key;

		[SerializeField]
		private EventType value;

		public EmitterType Key
		{
			get => key;
			set => key = value;
		}

		public EventType Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<EmitterType, EventType> other)
		{
			return other != null && other.Key == Key;
		}
	}
}