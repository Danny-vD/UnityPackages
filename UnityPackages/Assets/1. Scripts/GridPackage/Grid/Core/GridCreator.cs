using System;
using System.Collections.Generic;
using Enums.Grid;
using GridPackage.Grid.BaseTiles;
using GridPackage.ScriptableObjects;
using Structs.Utility.SerializableDictionary;
using UnityEngine;
using VDFramework;
using VDFramework.Extensions;
using VDFramework.UnityExtensions;

namespace GridPackage.Grid.Core
{
	public class GridCreator : BetterMonoBehaviour
	{
		[SerializeField]
		private TileTypePrefabs tileTypePrefabs;

		/// <summary>
		/// Generate the grid using the GridData attached to this gameobject; will return the grid as a 2d array of tiles
		/// </summary>
		public AbstractTile[,] GenerateGrid()
		{
			GridData data = GetComponent<GridData>();

			return data ? GenerateGrid(data, CachedTransform) : new AbstractTile[0, 0];
		}

		/// <summary>
		/// Generate the grid using the provided GridData; will return the grid as a 2d array of tiles
		/// </summary>
		public AbstractTile[,] GenerateGrid(GridData data, Transform parent)
		{
			DestroyGrid();

			AbstractTile[,] grid = new AbstractTile[data.GridSize.y, data.GridSize.x];

			foreach (SerializableKeyValuePair<Vector2Int, TileType> pair in data.TileData)
			{
				Vector2Int gridPosition = pair.Key;

				AbstractTile tile = InstantiateTile(data, parent, pair.Value, gridPosition);
				grid[gridPosition.y, gridPosition.x] = tile;
				AddNeighborsToTile(grid, tile);
			}

			data.SetGridEssentials(grid, parent);
			return grid;
		}

		/// <summary>
		/// Will fill in the 2d grid array and calculate the neighbors for a pre-existing grid in the scene
		/// </summary>
		/// <param name="gridData">The data that belongs to the grid</param>
		/// <param name="gridParent">The parent of this grid</param>
		public void CalculateExistingGrid(GridData gridData, Transform gridParent)
		{
			AbstractTile[] abstractTiles = gridParent.GetComponentsInChildren<AbstractTile>();

			Vector2Int gridSize = gridData.GridSize;

			// Create the 2d array and fill it with the abstractTiles we found in the scene
			AbstractTile[,] grid = new AbstractTile[gridSize.y, gridSize.x];

			foreach (AbstractTile tile in abstractTiles)
			{
				Vector2Int gridPosition = tile.GridPosition;

				if (GridUtil.IsValidPosition(gridData, gridPosition))
				{
					grid[gridPosition.y, gridPosition.x] = tile;
				}
			}

			// Check whether any coordinate in the 2d array is still null, which would mean we don't have all the tiles in the scene
			List<Vector2Int> unassignedPositions = new List<Vector2Int>(2);

			for (int y = 0; y < gridSize.y; y++)
			{
				for (int x = 0; x < gridSize.x; x++)
				{
					AbstractTile tile = grid[y, x];

					if (tile == null)
					{
						// No `break` so we can write all missing positions to the console
						unassignedPositions.Add(new Vector2Int(x, y));
					}
				}
			}

			// If we don't have all the necessary tiles, stop here and generate the grid as usual
			if (unassignedPositions.Count > 0)
			{
				string coordinates = unassignedPositions[0].ToString();

				for (int i = 1; i < unassignedPositions.Count; i++)
				{
					coordinates += "; " + unassignedPositions[i];
				}

				string message = unassignedPositions.Count == 1 ? $"position {{{coordinates}}} is" : $"positions {{{coordinates}}} are";

				Debug.LogWarning(
					$"Grid {message} not assigned!\nRegenerating grid...");
				GenerateGrid(gridData, gridData.transform);
				return;
			}

			// Loop over the entire grid again to set the neighbors correctly
			foreach (AbstractTile tile in grid)
			{
				AddNeighborsToTile(grid, tile);
			}

			// Update the GridData so that it has the correct 2d array and gridParent
			gridData.SetGridEssentials(grid, gridParent);
		}

		private AbstractTile InstantiateTile(GridData data, Transform parent, TileType type, Vector2Int gridPosition)
		{
			GameObject prefab = tileTypePrefabs.PrefabsPerTileType[type].GetRandomItem();

			if (prefab == null)
			{
				throw new NullReferenceException($"There is no prefab for {type}!");
			}

			GameObject instance = Instantiate(prefab, parent);
			instance.transform.position += CalculatePosition(data, parent, gridPosition);

			instance.name = $"{type.ToString()} {gridPosition.ToString()}";

			AbstractTile tile = instance.GetComponent<AbstractTile>();

			if (tile == null)
			{
				throw new NullReferenceException($"Prefab {prefab.name} does not have an {nameof(AbstractTile)}Component attached to it!");
			}

			tile.Initialize(gridPosition);

			return tile;
		}

		private static Vector3 CalculatePosition(GridData data, Transform parent, Vector2Int gridPosition)
		{
			Vector3 position = Vector3.zero;

			float deltaX = gridPosition.x * (data.TileSize.x + data.TileSpacing.x);
			float deltaZ = gridPosition.y * (data.TileSize.y + data.TileSpacing.y);

			switch (data.GridOrigin)
			{
				case GridOrigin.BottomLeft:
					position += parent.right * deltaX;
					position += parent.forward * deltaZ;
					break;
				case GridOrigin.TopLeft:
					position += parent.right * deltaX;
					position -= parent.forward * deltaZ;
					break;
				case GridOrigin.TopRight:
					position -= parent.right * deltaX;
					position -= parent.forward * deltaZ;
					break;
				case GridOrigin.BottomRight:
					position -= parent.right * deltaX;
					position += parent.forward * deltaZ;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(data.GridOrigin), "Grid Origin does not have a valid value!");
			}

			return position;
		}

		private static void AddNeighborsToTile(AbstractTile[,] grid, AbstractTile thisTile)
		{
			Vector2Int gridPosition = thisTile.GridPosition;

			// Make sure we can never sample below 0,0
			Vector2Int samplePosition = Vector2Int.zero;
			samplePosition.x = Mathf.Max(0, gridPosition.x - 1);
			samplePosition.y = Mathf.Max(0, gridPosition.y - 1);

			AbstractTile neighbor = grid[samplePosition.y, gridPosition.x]; // One below us, in the same column

			if (neighbor != thisTile)
			{
				thisTile.AddNeighbor(neighbor);
				neighbor.AddNeighbor(thisTile);
			}

			neighbor = grid[gridPosition.y, samplePosition.x]; // One before us, in the same row

			if (neighbor != thisTile)
			{
				thisTile.AddNeighbor(neighbor);
				neighbor.AddNeighbor(thisTile);
			}
		}

		[ContextMenu("Remove Grid")]
		public void DestroyGrid()
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
			{
				DestroyGridImmediate();
			}
			else
#endif
				CachedTransform.DestroyChildren();
		}

#if UNITY_EDITOR

		private void DestroyGridImmediate()
		{
			CachedTransform.DestroyChildrenImmediate();
		}
#endif
	}
}