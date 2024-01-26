using System;
using GridPackage.Enums.Grid;
using Structs.Utility.SerializableDictionary;
using UnityEngine;

namespace GridPackage.Comparisons
{
	public static class TileDataSorter
	{
		public static Comparison<SerializableKeyValuePair<Vector2Int, TileType>> GetSorter(GridOrigin gridOrigin)
		{
			return gridOrigin switch
			{
				GridOrigin.BottomLeft => BottomLeftComparer,
				GridOrigin.TopLeft => TopLeftComparer,
				GridOrigin.TopRight => TopRightComparer,
				GridOrigin.BottomRight => BottomRightComparer,
				_ => throw new ArgumentOutOfRangeException(nameof(gridOrigin), "Grid Origin does not have a valid value!"),
			};
		}
		
		private static int BottomLeftComparer(SerializableKeyValuePair<Vector2Int, TileType> me, SerializableKeyValuePair<Vector2Int, TileType> other)
		{
			// During generation, {0,0} is the bottom left, but in the editor window we start drawing in the top left
			// so to fix that, we flip the Y axis

			if (me.Key.Equals(other.Key))
			{
				return 0;
			}

			if (me.Key.y > other.Key.y)
			{
				return -1;
			}

			if (me.Key.y < other.Key.y)
			{
				return 1;
			}

			if (me.Key.x < other.Key.x)
			{
				return -1;
			}

			return 1;
		}

		private static int TopLeftComparer(SerializableKeyValuePair<Vector2Int, TileType> me, SerializableKeyValuePair<Vector2Int, TileType> other)
		{
			// During generation, {0,0} is the top left, and in the editor window we start drawing in the top left so we don't do any special sorting

			if (me.Key.Equals(other.Key))
			{
				return 0;
			}

			if (me.Key.y > other.Key.y)
			{
				return 1;
			}

			if (me.Key.y < other.Key.y)
			{
				return -1;
			}

			if (me.Key.x < other.Key.x)
			{
				return -1;
			}

			return 1;
		}
		
		private static int TopRightComparer(SerializableKeyValuePair<Vector2Int, TileType> me, SerializableKeyValuePair<Vector2Int, TileType> other)
		{
			// During generation, {0,0} is the top right, but in the editor window we start drawing in the top left
			// so to fix that, we flip the X axis

			if (me.Key.Equals(other.Key))
			{
				return 0;
			}

			if (me.Key.y > other.Key.y)
			{
				return 1;
			}

			if (me.Key.y < other.Key.y)
			{
				return -1;
			}

			if (me.Key.x < other.Key.x)
			{
				return 1;
			}

			return -1;
		}
		
		private static int BottomRightComparer(SerializableKeyValuePair<Vector2Int, TileType> me, SerializableKeyValuePair<Vector2Int, TileType> other)
		{
			// During generation, {0,0} is the bottom right, but in the editor window we start drawing in the top left
			// so to fix that, we flip the X and Y axis

			if (me.Key.Equals(other.Key))
			{
				return 0;
			}

			if (me.Key.y > other.Key.y)
			{
				return -1;
			}

			if (me.Key.y < other.Key.y)
			{
				return 1;
			}

			if (me.Key.x < other.Key.x)
			{
				return 1;
			}

			return -1;
		}
	}
}