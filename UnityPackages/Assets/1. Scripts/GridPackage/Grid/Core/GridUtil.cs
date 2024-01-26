using GridPackage.Enums.Grid;
using GridPackage.Grid.BaseTiles;
using UnityEngine;
using VDFramework;
using VDFramework.UnityExtensions;

namespace GridPackage.Grid.Core
{
	[RequireComponent(typeof(GridData))]
	public class GridUtil : BetterMonoBehaviour
	{
		[SerializeField, Tooltip("If true, generate the grid on Awake.\nOtherwise it will have to be manually started")]
		private bool setupGridOnPlay = true;

		[SerializeField, Tooltip("Tell the grid creator to generate a new grid.\nIf false, we tell the grid creator to try to work with any pre-generated grid instead")]
		private bool generateNewGrid = false;

		[SerializeField, Tooltip("[OPTIONAL] Tell the Grid Creator what the parent of the grid is.\nWill use our own transform if null")]
		private Transform gridParent;

		/// <summary>
		/// Set the parent of the Grid (this will not affect any generated grids)
		/// </summary>
		public Transform GridParent
		{
			get => gridParent;
			set => gridParent = value;
		}
		
		public GridData GridData { get; private set; }

		private void Awake()
		{
			// Use our own transform if the field was not assigned
			gridParent ??= CachedTransform;

			GridData = GetComponent<GridData>();

			if (setupGridOnPlay)
			{
				SetupGrid();
			}
		}

		public void SetupGrid()
		{
			SetupGrid(generateNewGrid);
		}
		
		public void SetupGrid(bool generateNew)
		{
			if (generateNew)
			{
				InstantiateGrid();
			}
			else
			{
				CalculateExistingGrid();
			}
		}

		public AbstractTile GetTileAtPosition(Vector2Int gridPosition)
		{
			if (IsValidPosition(gridPosition))
			{
				return GridData.Grid[gridPosition.y, gridPosition.x];
			}

			Debug.LogError("Invalid gridPosition: given position is not on the grid");
			return null;
		}

		public TileType GetTypeAtPosition(Vector2Int gridPosition)
		{
			return GridData.GetTypeAtPosition(gridPosition);
		}

		public bool IsValidPosition(Vector2Int gridPosition)
		{
			return IsValidPosition(GridData, gridPosition);
		}

		public static bool IsValidPosition(GridData gridData, Vector2Int gridPosition)
		{
			Vector2Int gridSize = gridData.GridSize;

			return gridPosition.x < gridSize.x    // smaller than maximum X
				   && gridPosition.y < gridSize.y // smaller than maximum Y
				   && gridPosition.x >= 0         // X bigger or equal to zero
				   && gridPosition.y >= 0;        // Y bigger or equal to zero
		}

		private void InstantiateGrid()
		{
			this.EnsureComponent<GridCreator>().GenerateGrid(GridData, gridParent);
		}

		private void CalculateExistingGrid()
		{
			this.EnsureComponent<GridCreator>().CalculateExistingGrid(GridData, gridParent);
		}
	}
}