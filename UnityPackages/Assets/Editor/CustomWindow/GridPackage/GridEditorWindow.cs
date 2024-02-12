using System.Collections.Generic;
using GridPackage.Comparisons;
using GridPackage.Enums.Grid;
using GridPackage.Grid.Core;
using Structs.Utility.SerializableDictionary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Utility.UtilityPackage.EditorUtils;

namespace CustomWindow.GridPackage
{
	public class GridEditorWindow : EditorWindow
	{
		[MenuItem("Level Editor/Grid Editor")]
		public static void ShowWindow()
		{
			CreateWindow<GridEditorWindow>("Level Editor");
		}

		private Vector2 scroll;

		private GridData gridData;

		private void OnGUI()
		{
			AskForData();

			if (gridData == null)
			{
				return;
			}

			DrawGridProperties();

			DrawGenerateButton();
			
			scroll = EditorGUILayout.BeginScrollView(scroll, true, true);
			{
				EditorGUILayout.Space(20.0f);

				gridData.VerifyTileData();
				DrawTileData();

				EditorGUILayout.Space(20.0f);
				DrawRemoveGridButton();
			}
			EditorGUILayout.EndScrollView();
		}

		private void AskForData()
		{
			gridData = (GridData)EditorGUILayout.ObjectField("GridData", gridData, typeof(GridData), true);
		}

		private void DrawGridProperties()
		{
			EnumPopup(ref gridData.GridOrigin, "{0,0} Position");

			EditorGUILayout.BeginHorizontal();
			{
				FlexibleLabel("Grid Dimensions");
				gridData.GridSize = DrawVector2Int(gridData.GridSize.x, "X", gridData.GridSize.y, "Y");
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				FlexibleLabel("Tile Size");
				gridData.TileSize = DrawVector2(gridData.TileSize.x, "X", gridData.TileSize.y, "Y");
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				FlexibleLabel("Tile Spacing");
				gridData.TileSpacing = DrawVector2(gridData.TileSpacing.x, "X", gridData.TileSpacing.y, "Y");
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawGenerateButton()
		{
			if (!GUILayout.Button("Generate Grid", EditorStyles.miniButtonMid)) return;

			gridData.GetComponent<GridCreator>().GenerateGrid(gridData, gridData.transform);

			if (!EditorApplication.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}

		private void DrawTileData()
		{
			List<SerializableKeyValuePair<Vector2Int, TileType>> tileData = gridData.TileData;
			int length = tileData.Count;

			EditorGUILayout.BeginHorizontal();

			// position.width = window width
			float maxWidth = Mathf.Max(80, CalculateMaxItemSizeForLimit(gridData.GridSize.x, position.width));

			// Sort so that the 2d grid is correctly drawn with respect to the GridOrigin
			tileData.Sort(TileDataSorter.GetSorter(gridData.GridOrigin));

			for (int i = 0; i < length; i++)
			{
				if (i % gridData.GridSize.x == 0)
				{
					// Begin new line
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
				}

				SerializableKeyValuePair<Vector2Int, TileType> tileDatum = tileData[i];
				TileType oldTileType = tileDatum.Value;

				tileDatum.Value = (TileType)EditorGUILayout.EnumPopup(oldTileType, GUILayout.MaxWidth(maxWidth));

				if (oldTileType != tileDatum.Value)
				{
					EditorUtility.SetDirty(gridData);
				}

				gridData.ChangeTile(tileDatum.Key, tileDatum.Value);
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawRemoveGridButton()
		{
			if (!GUILayout.Button("Remove Grid", EditorStyles.miniButtonMid)) return;

			gridData.GetComponent<GridCreator>().DestroyGrid();

			if (!EditorApplication.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}

		private static float CalculateMaxItemSizeForLimit(int amountOfItems, float limit)
		{
			return limit / amountOfItems;
		}
	}
}