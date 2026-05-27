using UnityEngine;
using VDPackages.LootTablePackage.Baseclasses;

namespace VDPackages.LootTablePackage
{
	[CreateAssetMenu(menuName = "LootTables/Create Prefab LootTable", fileName = "Prefab LootTable", order = 0)]
	public class PrefabWeightedLootTable : WeightedLootTableObject<GameObject>
	{
	}
}