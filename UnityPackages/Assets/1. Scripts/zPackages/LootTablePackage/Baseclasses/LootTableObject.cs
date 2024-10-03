using UnityEngine;
using VDFramework.LootTables;
using VDFramework.LootTables.Interfaces;
using VDFramework.LootTables.LootTableItems;
using VDFramework.RandomWrapper.Interface;

namespace LootTablePackage.Baseclasses
{
	public abstract class LootTableObject<TLootType> : ScriptableObject, ILoot<TLootType>
	{
		public IRandomNumberGenerator RandomNumberGenerator
		{
			get => GetLootTable().RandomNumberGenerator;
			set => GetLootTable().RandomNumberGenerator = value;
		}
		
		protected WeightedLootTable<TLootType> lootTable;
		
		protected abstract WeightedLootTable<TLootType> GetNewLootTable();
		
		public WeightedLootTable<TLootType> GetLootTable()
		{
			return lootTable ??= GetNewLootTable();
		}

		/// <inheritdoc cref="VDFramework.LootTables.WeightedLootTable{TLootType}.GetLoot()"/>
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