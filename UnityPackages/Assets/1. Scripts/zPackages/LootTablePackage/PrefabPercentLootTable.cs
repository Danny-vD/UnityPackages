using LootTablePackage.Baseclasses;
using UnityEngine;

namespace LootTablePackage
{
	[CreateAssetMenu(menuName = "PercentLootTables/Create Prefab LootTable", fileName = "Prefab PercentedLootTable", order = 0)]
	public class PrefabPercentLootTable : PercentageLootTableObject<GameObject>
	{
	}
}