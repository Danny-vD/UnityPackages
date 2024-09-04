using System.Collections.Generic;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using VDFramework.LootTables;
using VDFramework.LootTables.Variations;

namespace LootTablePackage.Baseclasses
{
	public abstract class PercentageLootTableObject<TLootType> : LootTableObject<TLootType>
	{
		[SerializeField]
		private SerializableDictionary<TLootType, float> percentageLootTable;

		[SerializeField, Tooltip("This is part of the lootTable, use this for putting a lootTable inside a lootTable")]
		private SerializableDictionary<LootTableObject<TLootType>, float> nestedLootTables;

		protected override WeightedLootTable<TLootType> GetNewLootTable()
		{
			PercentageLootTable<TLootType> weightedLootTable = new PercentageLootTable<TLootType>(percentageLootTable);

			foreach (KeyValuePair<LootTableObject<TLootType>, float> pair in nestedLootTables)
			{
				weightedLootTable.TryAdd(pair.Key, pair.Value);
			}

			return weightedLootTable;
		}
	}
}