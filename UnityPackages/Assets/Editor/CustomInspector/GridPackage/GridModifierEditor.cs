using System.Collections.Generic;
using System.Linq;
using GridPackage.Enums.Grid;
using GridPackage.Grid.BaseTiles;
using GridPackage.Grid.Core;
using GridPackage.Grid.EditorTools;
using UnityEditor;
using UnityEngine;
using static Utility.EditorPackage.EditorUtils;

namespace CustomInspector.GridPackage
{
	[CustomEditor(typeof(GridModifier))]
	public class GridModifierEditor : Editor
	{
		private GridModifier gridModifier;
		private static bool regenerateGrid = true;

		private bool showSelectedFoldout;

		//////////////////////////////////////////////////

		private SerializedProperty selectedPositions;
		private SerializedProperty newType;

		//////////////////////////////////////////////////

		private bool hasGenerated = false;

		private void OnEnable()
		{
			gridModifier = target as GridModifier;

			selectedPositions = serializedObject.FindProperty("SelectedPositions");
			newType           = serializedObject.FindProperty("newType");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GetAllSelectedTiles();

			regenerateGrid = EditorGUILayout.Toggle("Generate on edit", regenerateGrid);

			DrawNewType();

			DrawModifyTilesButton();

			DrawSelectedPositions();

			serializedObject.ApplyModifiedProperties();
		}

		private void GetAllSelectedTiles()
		{
			GameObject[] selectedObjects = Selection.GetFiltered<GameObject>(SelectionMode.Unfiltered);

			List<AbstractTile> selectedTiles = selectedObjects.Select(GetAbstractTile).ToList();
			selectedTiles.RemoveAll(item => item == null);
			selectedTiles = selectedTiles.Distinct().ToList();

			Vector2Int[] gridPositions = new Vector2Int[selectedTiles.Count];

			for (int i = 0; i < selectedTiles.Count; i++)
			{
				AbstractTile tile = selectedTiles[i];
				gridPositions[i] = tile.GridPosition;
			}

			// Very first time all grid posititons are (0,0), despite being distinct tiles
			// In this case, regenerate the grid
			if (!hasGenerated && gridPositions.Length > 0)
			{
				if (gridPositions.All(position => position.Equals(Vector2Int.zero)))
				{
					gridModifier.GetComponent<GridCreator>().GenerateGrid();
					hasGenerated = true;
					GetAllSelectedTiles();
					return;
				}

				hasGenerated = true;
			}

			gridModifier.SelectedPositions = gridPositions;

			AbstractTile GetAbstractTile(GameObject gameObject)
			{
				AbstractTile tile = gameObject.GetComponent<AbstractTile>();

				if (tile == null)
				{
					tile = gameObject.GetComponentInParent<AbstractTile>();
				}

				return tile;
			}
		}

		private void DrawNewType()
		{
			TileType tileType = (TileType)newType.enumValueIndex;
			newType.enumValueIndex = EnumPopup(ref tileType, "New type");
		}

		private void DrawModifyTilesButton()
		{
			if (!GUILayout.Button("Modify tiles", EditorStyles.miniButtonMid)) return;

			gridModifier.ModifyTiles(regenerateGrid);
		}

		private void DrawSelectedPositions()
		{
			int length = selectedPositions.arraySize;

			if (IsFoldOut(ref showSelectedFoldout, $"Selected Positions    [{length}]"))
			{
				for (int i = 0; i < length; i++)
				{
					SerializedProperty gridPosition = selectedPositions.GetArrayElementAtIndex(i); // Vector2Int

					EditorGUILayout.LabelField(gridPosition.vector2IntValue.ToString());
				}
			}
		}
	}
}