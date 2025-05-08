using System;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;

namespace FMODUtilityPackage.Structs
{
	[Serializable]
	public struct EventsPerEmitter : IKeyValuePair<GlobalEmitter, AudioEvent>
	{
		[SerializeField]
		private GlobalEmitter key;

		[SerializeField]
		private AudioEvent value;

		public GlobalEmitter Key
		{
			get => key;
			set => key = value;
		}

		public AudioEvent Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<GlobalEmitter, AudioEvent> other)
		{
			return other != null && other.Key == Key;
		}
	}
}