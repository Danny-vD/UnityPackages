using System;
using UnityEngine;
using VDFramework.Interfaces;
using VDPackages.FMODUtilityPackage.Enums;

namespace VDPackages.FMODUtilityPackage.Structs
{
	[Serializable]
	public struct BusPathPerBus : IKeyValuePair<AudioBus, string>
	{
		[SerializeField]
		private AudioBus key;

		[SerializeField]
		private string value;

		public AudioBus Key
		{
			get => key;
			set => key = value;
		}

		public string Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<AudioBus, string> other)
		{
			return other != null && other.Key == Key;
		}
	}
}