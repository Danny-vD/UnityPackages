using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VDFramework.Interfaces;

namespace SerializableDictionaryPackage.Structs
{
	[Serializable]
	public struct SerializableKeyValuePair<TKey, TValue> : IKeyValuePair<TKey, TValue>, IEquatable<SerializableKeyValuePair<TKey, TValue>>
	{
		public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> pair)
		{
			return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
		}

		public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
		{
			return new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value);
		}

		public static bool operator ==(SerializableKeyValuePair<TKey, TValue> lhs, SerializableKeyValuePair<TKey, TValue> rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(SerializableKeyValuePair<TKey, TValue> lhs, SerializableKeyValuePair<TKey, TValue> rhs)
		{
			return !lhs.Equals(rhs);
		}
		
		public static bool operator ==(SerializableKeyValuePair<TKey, TValue> lhs, IKeyValuePair<TKey, TValue> rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(SerializableKeyValuePair<TKey, TValue> lhs, IKeyValuePair<TKey, TValue> rhs)
		{
			return !lhs.Equals(rhs);
		}

		[SerializeField]
		private TKey key;

		[SerializeField]
		private TValue value;

		public TKey Key
		{
			get => key;
			set => key = value;
		}

		public TValue Value
		{
			get => value;
			set => this.value = value;
		}

		public SerializableKeyValuePair(TKey pairKey, TValue pairValue)
		{
			key   = pairKey;
			value = pairValue;
		}

		public bool Equals(IKeyValuePair<TKey, TValue> other) => other != null && Key != null && Key.Equals(other.Key);

		public bool Equals(SerializableKeyValuePair<TKey, TValue> other) => Key != null && Key.Equals(other.Key);

		public override bool Equals(object obj)
		{
			if (obj is not IKeyValuePair<TKey, TValue> pair)
			{
				return false;
			}
		
			return Key.Equals(pair.Key);
		}

		public override int GetHashCode() => EqualityComparer<TKey>.Default.GetHashCode(key);

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Clear();
			sb.Append('[').Append(key).Append(", ").Append(value).Append(']');
			return sb.ToString();
		}
	}
}