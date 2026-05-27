using System;
using UnityEngine;
using VDFramework.Interfaces;
using VDPackages.FMODUtilityPackage.Enums;

namespace VDPackages.FMODUtilityPackage.Structs
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