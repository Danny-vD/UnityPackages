using System.Collections.Generic;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using VDFramework.LootTables;

namespace LootTablePackage.Baseclasses
{
	public abstract class WeightedLootTableObject<TLootType> : LootTableObject<TLootType>
	{
		[SerializeField]
		private SerializableDictionary<TLootType, long> weightedLootTable;

		[SerializeField, Tooltip("This is part of the lootTable, use this for putting a lootTable inside a lootTable")]
		private SerializableDictionary<LootTableObject<TLootType>, long> nestedLootTables;

		protected override WeightedLootTable<TLootType> GetNewLootTable()
		{
			WeightedLootTable<TLootType> table = new WeightedLootTable<TLootType>(weightedLootTable);

			foreach (KeyValuePair<LootTableObject<TLootType>, long> pair in nestedLootTables)
			{
				table.TryAdd(pair.Key, pair.Value);
			}

			return table;
		}
	}
}