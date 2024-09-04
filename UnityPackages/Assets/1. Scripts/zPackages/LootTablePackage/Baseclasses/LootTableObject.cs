using UnityEngine;
using VDFramework.LootTables;
using VDFramework.LootTables.Interfaces;
using VDFramework.LootTables.LootTableItems;

namespace LootTablePackage.Baseclasses
{
	public abstract class LootTableObject<TLootType> : ScriptableObject, ILoot<TLootType>
	{
		protected WeightedLootTable<TLootType> lootTable;
		
		protected abstract WeightedLootTable<TLootType> GetNewLootTable();
		
		public WeightedLootTable<TLootType> GetLootTable()
		{
			return lootTable ??= GetNewLootTable();
		}

		public virtual TLootType GetLoot()
		{
			return GetLootTable().GetLoot();
		}
		
		public decimal GetLootDropChance(TLootType loot)
		{
			ILoot<TLootType> tableItem = ConvertToLoot(loot);
			return GetLootTable().GetLootDropChance(tableItem);
		}

		protected static ILoot<TLootType> ConvertToLoot(TLootType loot)
		{
			return new LootTableItem<TLootType>(loot);
		}
	}
}