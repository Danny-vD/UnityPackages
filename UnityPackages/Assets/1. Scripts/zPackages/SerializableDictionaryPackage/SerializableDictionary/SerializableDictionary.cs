using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SerializableDictionaryPackage.Structs;
using UnityEngine;

namespace SerializableDictionaryPackage.SerializableDictionary
{
	/// <summary>
	/// A 'fake' dictionary that can be serialized
	/// </summary>
	[Serializable, DebuggerDisplay("Count = {Count}")]
	public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, ISerializationCallbackReceiver
	{
		#region Fields

		[SerializeField]
		protected List<SerializableKeyValuePair<TKey, TValue>> serializedDictionary = new List<SerializableKeyValuePair<TKey, TValue>>();

		protected List<SerializableKeyValuePair<TKey, TValue>> InternalList = new List<SerializableKeyValuePair<TKey, TValue>>();

		#endregion

		#region Operators

		public static explicit operator SerializableDictionary<TKey, TValue>(List<SerializableKeyValuePair<TKey, TValue>> list)
		{
			return new SerializableDictionary<TKey, TValue>(list);
		}

		public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
		{
			return new SerializableDictionary<TKey, TValue>(dictionary);
		}

		public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serializableDictionary)
		{
			return serializableDictionary.InternalList.ToDictionary(pair => pair.Key, keyValuePair => keyValuePair.Value);
		}

		#endregion

		#region Properties

		public TValue this[TKey key]
		{
			get
			{
				TryGetValue(key, out TValue value);
				return value;
			}
			set => Add(key, value);
		}

		public SerializableKeyValuePair<TKey, TValue> this[int index]
		{
			get => InternalList[index];
			set
			{
				InternalList[index]         = value;
				serializedDictionary[index] = value;
			}
		}

		public object this[object key]
		{
			get
			{
				TryGetValue(key, out TValue value);
				return value;
			}
			set => ((IDictionary)this).Add(key, value);
		}

		public int Count => InternalList.Count;

		public object SyncRoot => ((ICollection)InternalList).SyncRoot;

		public bool IsFixedSize => false;
		public bool IsSynchronized => false;
		public bool IsReadOnly => false;

		public ICollection<TKey> Keys => InternalList.Select(pair => pair.Key).ToArray();

		public ICollection<TValue> Values => InternalList.Select(pair => pair.Value).ToArray();

		ICollection IDictionary.Values => (ICollection)Keys;

		ICollection IDictionary.Keys => (ICollection)Values;

		#endregion

		#region Constructors

		public SerializableDictionary()
		{
		}

		public SerializableDictionary(IEnumerable<SerializableKeyValuePair<TKey, TValue>> list)
		{
			InternalList         = list.Distinct().ToList();
			serializedDictionary = InternalList;
		}

		public SerializableDictionary(params SerializableKeyValuePair<TKey, TValue>[] keyValuePairs) : this(keyValuePairs.Distinct())
		{
		}

		public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
		{
			foreach (KeyValuePair<TKey, TValue> pair in dictionary)
			{
				Add(pair.Key, pair.Value);
			}
		}

		public SerializableDictionary(SerializableDictionary<TKey, TValue> dictionary)
		{
			foreach (KeyValuePair<TKey, TValue> pair in dictionary)
			{
				Add(pair.Key, pair.Value);
			}
		}

		#endregion

		#region IDictionary

		public void Add(TKey key, TValue value)
		{
			int index = FindEntry(key);

			// FindIndex returns -1 if it's not present
			if (index < 0)
			{
				SerializableKeyValuePair<TKey, TValue> newPair = new SerializableKeyValuePair<TKey, TValue>(key, value);
				InternalList.Add(newPair);
				serializedDictionary.Add(newPair);
				return;
			}

			SerializableKeyValuePair<TKey, TValue> pair = InternalList[index];
			pair.Value = value;

			InternalList[index]         = pair;
			serializedDictionary[index] = pair;
		}

		public bool Remove(TKey key)
		{
			int index = FindEntry(key);

			if (index < 0)
			{
				return false;
			}

			InternalList.RemoveAt(index);
			serializedDictionary.RemoveAt(index);
			return true;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int index = FindEntry(key);

			if (index >= 0)
			{
				value = InternalList[index].Value;
				return true;
			}

			value = default;
			return false;
		}

		void IDictionary.Add(object key, object value)
		{
			VerifyKey(key);

			VerifyValue(value);

			Add((TKey)key, (TValue)value);
		}

		void IDictionary.Remove(object key)
		{
			VerifyKey(key);

			Remove((TKey)key);
		}

		bool IDictionary.Contains(object key)
		{
			return VerifyKey(key) && ContainsKey((TKey)key);
		}

		#endregion

		#region ICollection

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			SerializableKeyValuePair<TKey, TValue> pair = item;

			// ReSharper disable once InvertIf | Reason: I prefer it like this
			if (!InternalList.Contains(pair))
			{
				InternalList.Add(pair);
				serializedDictionary.Add(pair);
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			SerializableKeyValuePair<TKey, TValue> pair = item;

			serializedDictionary.Remove(pair);
			return InternalList.Remove(pair);
		}

		public void Clear()
		{
			InternalList.Clear();
			serializedDictionary.Clear();
		}
		
		// ReSharper disable once UsageOfDefaultStructEquality | False positive, it actually uses the custom Equals function from SerializableKeyValuePair<,>
		public bool Contains(KeyValuePair<TKey, TValue> item) => InternalList.Contains(item);

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			ToKeyValuePair().ToList().CopyTo(array, arrayIndex);
		}

		public void CopyTo(Array array, int index)
		{
			((ICollection)InternalList).CopyTo(array, index);
		}

		#endregion

		#region ISerializationCallbackReceiver

		public virtual void OnBeforeSerialize()
		{
		}

		public virtual void OnAfterDeserialize()
		{
			InternalList = serializedDictionary.Distinct().ToList();
		}

		#endregion

		#region IEnumerable

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
			ToKeyValuePair().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IDictionaryEnumerator IDictionary.GetEnumerator() => ((Dictionary<TKey, TValue>)this).GetEnumerator();

		#endregion

		#region Public

		public bool TryGetValue(object key, out TValue value)
		{
			VerifyKey(key);

			int index = FindEntry((TKey)key);

			if (index >= 0)
			{
				value = InternalList[index].Value;
				return true;
			}

			value = default;
			return false;
		}

		public bool ContainsKey(TKey key)
		{
			return FindEntry(key) >= 0;
		}

		public bool ContainsValue(TValue value)
		{
			return InternalList.Any(pair => pair.Value.Equals(value));
		}

		#endregion

		#region Private

		private static bool VerifyKey(object key)
		{
			switch (key)
			{
				case null when typeof(TKey).IsValueType:
					throw new ArgumentNullException(nameof(key), $"null while {typeof(TKey).Name} cannot be null");
				case null:
				case TKey _:
					return true;
				default:
					throw new ArgumentException($"{key} is not of type {typeof(TKey).Name}", nameof(key));
			}
		}

		private static bool VerifyValue(object value)
		{
			switch (value)
			{
				case null when typeof(TValue).IsValueType:
					throw new ArgumentNullException(nameof(value), $"null while {typeof(TValue).Name} cannot be null");
				case null:
				case TValue _:
					return true;
				default:
					throw new ArgumentException($"{value} is not of type {typeof(TValue).Name}", nameof(value));
			}
		}

		private IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePair()
		{
			return InternalList.Select(pair => (KeyValuePair<TKey, TValue>)pair);
		}

		private SerializableKeyValuePair<TKey, TValue> GetPair(TKey key)
		{
			return InternalList.First(pair => pair.Key.Equals(key));
		}

		/// <summary>
		/// Returns the index of the dictionaryEntry of a given key
		/// </summary>
		private int FindEntry(TKey key)
		{
			int index = InternalList.FindIndex(pair => pair.Key.Equals(key));
			return index;
		}

		#endregion
	}
}