using System;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;
using EventType = FMODUtilityPackage.Enums.EventType;

namespace FMODUtilityPackage.Structs
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