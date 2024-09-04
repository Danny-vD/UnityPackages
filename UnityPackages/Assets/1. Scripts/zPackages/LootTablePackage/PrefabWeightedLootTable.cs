using LootTablePackage.Baseclasses;
using UnityEngine;

namespace LootTablePackage
{
	[CreateAssetMenu(menuName = "LootTables/Create Prefab LootTable", fileName = "Prefab LootTable", order = 0)]
	public class PrefabWeightedLootTable : WeightedLootTableObject<GameObject>
	{
	}
}