using System.Collections.Generic;
using System.Linq;
using Enums.Grid;
using GridPackage.Grid.BaseTiles;
using Structs.Utility.SerializableDictionary;
using UnityEngine;
using VDFramework;
using VDFramework.Extensions;

namespace GridPackage.Grid.Core
{
	public class GridData : BetterMonoBehaviour
	{
		[SerializeField]
		private List<SerializableKeyValuePair<Vector2Int, TileType>> tileData = new List<SerializableKeyValuePair<Vector2Int, TileType>>();
		
		public Transform GridParent { get; private set; }
		public AbstractTile[,] Grid { get; private set; }

		public GridOrigin GridOrigin = GridOrigin.BottomLeft;
		
		public Vector2Int GridSize = Vector2Int.one;

		[Tooltip("This is the size of the prefab that is used for the tile")]
		public Vector2 TileSize = new Vector2(10, 10); // 10,10 is the default plane size

		public Vector2 TileSpacing = Vector2.zero;

		/// <summary>
		/// A copy of the TileData
		/// </summary>
		public List<SerializableKeyValuePair<Vector2Int, TileType>> TileData => new List<SerializableKeyValuePair<Vector2Int, TileType>>(tileData);

		public void SetGridEssentials(AbstractTile[,] grid, Transform gridParent)
		{
			Grid       = grid;
			GridParent = gridParent;
		}

		/// <summary>
		/// Set the TileType of a tile at a given position
		/// </summary>
		public void ChangeTile(Vector2Int gridPosition, TileType newType)
		{
			int length = tileData.Count;
			
			for (int i = 0; i < length; i++)
			{
				SerializableKeyValuePair<Vector2Int, TileType> pair = tileData[i];

				if (pair.Key.Equals(gridPosition))
				{
					pair.Value  = newType;
					tileData[i] = pair;
					break;
				}
			}
		}
		
		public TileType GetTypeAtPosition(Vector2Int gridPosition)
		{
			if (GridUtil.IsValidPosition(this, gridPosition))
			{
				SerializableKeyValuePair<Vector2Int, TileType> pair = tileData.First(pair => gridPosition.Equals(pair.Key));

				return pair.Value;
			}

			Debug.LogError("Invalid gridPosition: given position is not on the grid");
			return (TileType)(-1);
		}

		public void VerifyTileData()
		{
			Vector2Int dimensions = GridSize;
			int tileCount = dimensions.x * dimensions.y;

			if (tileData.Count == tileCount) // Everything is fine
			{
				return;
			}
			
			tileData.ResizeList(tileCount);

			for (int i = 0; i < tileCount; i++)
			{
				int x = i % dimensions.x;
				int y = i / dimensions.x;
				
				SerializableKeyValuePair<Vector2Int, TileType> @struct = tileData[i];
				@struct.Key = new Vector2Int(x, y);
				tileData[i] = @struct;
			}
		}
	}
}