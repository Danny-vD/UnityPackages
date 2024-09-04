using System;
using FMODUtilityPackage.Enums;
using UnityEngine;
using VDFramework.Interfaces;

namespace FMODUtilityPackage.Structs
{
	[Serializable]
	public struct BusPathPerBus : IKeyValuePair<BusType, string>
	{
		[SerializeField]
		private BusType key;

		[SerializeField]
		private string value;

		public BusType Key
		{
			get => key;
			set => key = value;
		}

		public string Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<BusType, string> other)
		{
			return other != null && other.Key == Key;
		}
	}
}