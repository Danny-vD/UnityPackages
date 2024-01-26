using System;
using System.Collections.Generic;
using System.Linq;
using Structs.Utility.SerializableDictionary;
using VDFramework.Utility;

namespace SerializableDictionaryPackage.SerializableDictionary
{
	[Serializable]
	public class SerializableEnumDictionary<TKey, TValue> : SerializableDictionary<TKey, TValue>
		where TKey : struct, Enum
	{
		public static implicit operator SerializableEnumDictionary<TKey, TValue>(List<SerializableKeyValuePair<TKey, TValue>> list)
		{
			return new SerializableEnumDictionary<TKey, TValue>(list);
		}

		public static implicit operator SerializableEnumDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
		{
			return new SerializableEnumDictionary<TKey, TValue>(dictionary);
		}
		
		public static implicit operator Dictionary<TKey, TValue>(SerializableEnumDictionary<TKey, TValue> serializableDictionary)
		{
			return serializableDictionary.InternalList.ToDictionary(pair => pair.Key, keyValuePair => keyValuePair.Value);
		}

		public SerializableEnumDictionary(IEnumerable<SerializableKeyValuePair<TKey, TValue>> list) : base(list)
		{
		}

		public SerializableEnumDictionary(params SerializableKeyValuePair<TKey, TValue>[] keyValuePairs) : base(keyValuePairs.Distinct())
		{
		}

		public SerializableEnumDictionary(Dictionary<TKey, TValue> dictionary) : base(dictionary)
		{
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();
			Populate();
		}
		
		/// <summary>
		/// Automatically fills the dictionary with an entry for every enum value
		/// </summary>
		private void Populate()
		{
			EnumDictionaryUtil.PopulateEnumDictionary<SerializableKeyValuePair<TKey, TValue>, TKey, TValue>(serializedDictionary);
		}
	}
}