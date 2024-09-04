using System;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;

namespace FMODUtilityPackage.Structs
{
	[Serializable]
	public struct EventsPerEmitter : IKeyValuePair<EmitterType, AudioEventType>
	{
		[SerializeField]
		private EmitterType key;

		[SerializeField]
		private AudioEventType value;

		public EmitterType Key
		{
			get => key;
			set => key = value;
		}

		public AudioEventType Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<EmitterType, AudioEventType> other)
		{
			return other != null && other.Key == Key;
		}
	}
}