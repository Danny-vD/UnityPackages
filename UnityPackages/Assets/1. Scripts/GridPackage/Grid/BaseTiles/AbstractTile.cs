using System.Collections.Generic;
using GridPackage.Enums.Grid;
using GridPackage.Structs;
using UnityEngine;
using UtilityPackage.Attributes;
using VDFramework;

namespace GridPackage.Grid.BaseTiles
{
	public abstract class AbstractTile : BetterMonoBehaviour
	{
		[SerializeField, ReadOnly]
		private Vector2Int gridPosition = new Vector2Int(-1, -1);
		
		public Vector2Int GridPosition
		{
			get => gridPosition;
			private set => gridPosition = value;
		}
		
		[SerializeField]
		protected TileProperties properties = TileProperties.Default;
		
		public TileProperties Properties => properties;

		public abstract TileType TileType { get; }
		
		// Set the underlaying array to 4, as that is the most common amount of neighbors
		protected readonly List<AbstractTile> Neighbors = new List<AbstractTile>(4);
		
		// ReSharper disable once Unity.RedundantEventFunction
		// Discourage any derivative classes from using the Awake method (because the tile is not properly initialized at this moment)
		// Override Initialize instead
		protected void Awake()
		{
		}
		
		/// <summary>
		/// Set the GridPosition of this tile, intended to only set after instantiating the tile
		/// </summary>
		/// <param name="position"></param>
		public virtual void Initialize(Vector2Int position)
		{
			GridPosition = position;
		}

		/// <summary>
		/// Tell the tile it has a new neighbor
		/// </summary>
		public virtual void AddNeighbor(AbstractTile tile)
		{
			if (Neighbors.Contains(tile))
			{
				return;
			}

			Neighbors.Add(tile);
		}
	}
}