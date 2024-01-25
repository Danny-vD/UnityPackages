using System;
using Enums.Grid;
using GridPackage.Grid.Core;
using UnityEngine;
using Utility.EditorHelpers;

namespace GridPackage.Grid.EditorTools
{
	/// <summary>
	/// A class specifically made to be used in the insector to modify the grid
	/// </summary>
	[DisallowMultipleComponent]
	public class GridModifier : RemoveInBuild
	{
		[SerializeField]
		private TileType newType = default;

		public Vector2Int[] SelectedPositions = Array.Empty<Vector2Int>();

		[SerializeField, Tooltip("[OPTIONAL] Tell the Grid Creator what the parent of the grid is.\nWill use our own transform if null")]
		private Transform gridParent;
		
		[SerializeField, Tooltip("[OPTIONAL] Use a specific grid data.\nIf null: will use the grid data on the current object if one exists")]
		private GridData data = null;
		
		[SerializeField, Tooltip("[OPTIONAL] Use a specific grid creator.\nIf null: will use a Grid Creator on the current object if one exists")]
		private GridCreator gridCreator = null;

		protected override void Awake()
		{
			base.Awake();

			gridParent  ??= CachedTransform;
			data        ??= GetComponent<GridData>();
			gridCreator ??= GetComponent<GridCreator>();
		}

		/// <summary>
		/// Change all selected tiles to the new tileType
		/// </summary>
		/// <param name="regenerateGrid">Regenerate the grid after setting the new tiles</param>
		public void ModifyTiles(bool regenerateGrid)
		{
			if (!data)
			{
				data = GetComponent<GridData>();
			}

			foreach (Vector2Int position in SelectedPositions)
			{
				data.ChangeTile(position, newType);
			}

			if (regenerateGrid)
			{
				RegenerateGrid();
			}
		}

		private void RegenerateGrid()
		{
			if (!gridCreator)
			{
				gridCreator = GetComponent<GridCreator>();
			}

			gridCreator.GenerateGrid(data, gridParent);
		}
	}
}