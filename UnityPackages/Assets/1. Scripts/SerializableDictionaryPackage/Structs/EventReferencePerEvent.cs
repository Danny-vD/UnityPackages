using System;
using FMODUnity;
using UnityEngine;
using VDFramework.Interfaces;
using EventType = FMODUtilityPackage.Enums.EventType;

namespace FMODUtilityPackage.Structs
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