using GridPackage.Enums.Grid;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;

namespace GridPackage.ScriptableObjects
{
    /// <summary>
    /// Used to tell the GridCreator which prefabs to use for each TileType
    /// </summary>
    [CreateAssetMenu(fileName = nameof(TileTypePrefabs), menuName = "Grid/TileTypePrefabData")]
    public class TileTypePrefabs : ScriptableObject 
    {
        [Header("A random prefab will be taken from the list if it has more than one prefab")]
        public SerializableEnumDictionary<TileType, GameObject[]> PrefabsPerTileType;
    }
}
