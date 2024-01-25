using Enums.Grid;
using GridPackage.Grid.Core;
using UnityEditor;
using UnityEngine;
using static Utility.EditorUtils;

namespace CustomInspector.Grid
{
	[CustomEditor(typeof(GridData), true)]
	public class GridDataEditor : UnityEditor.Editor
	{
		private static bool tileDataFoldout;

		//////////////////////////////////////////////////

		private SerializedProperty gridOrigin;
		private SerializedProperty gridSize;
		private SerializedProperty tileSize;
		private SerializedProperty tileSpacing;

		private SerializedProperty tileData;

		private void OnEnable()
		{
			gridOrigin  = serializedObject.FindProperty("GridOrigin");
			gridSize    = serializedObject.FindProperty("GridSize");
			tileSize    = serializedObject.FindProperty("TileSize");
			tileSpacing = serializedObject.FindProperty("TileSpacing");
			tileData    = serializedObject.FindProperty("tileData");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(gridOrigin, new GUIContent("{0,0} Position"));
			EditorGUILayout.PropertyField(gridSize);
			EditorGUILayout.PropertyField(tileSize);
			EditorGUILayout.PropertyField(tileSpacing);

			VerifyTileData();
			DrawTileData();

			serializedObject.ApplyModifiedProperties();
		}

		private void VerifyTileData()
		{
			GridData gridData = (GridData)target;
			gridData.VerifyTileData();
		}

		private void DrawTileData()
		{
			if (!IsFoldOut(ref tileDataFoldout, "Tile Data")) // Shows the foldout label
			{
				return;
			}

			++EditorGUI.indentLevel;

			for (int i = 0; i < tileData.arraySize; i++)
			{
				SerializedProperty keyValuePair = tileData.GetArrayElementAtIndex(i);
				SerializedProperty position = keyValuePair.FindPropertyRelative("key");   // Vector2Int
				SerializedProperty tileType = keyValuePair.FindPropertyRelative("value"); // TileType
				
				Vector2Int positionVector = position.vector2IntValue;
				FlexibleLabel("{" + positionVector.x + "," + positionVector.y + "}");

				TileType type = ConvertIntToEnum<TileType>(tileType.enumValueIndex);

				tileType.enumValueIndex = EnumPopup(ref type, string.Empty);

				EditorGUILayout.Space();
			}

			--EditorGUI.indentLevel;
		}
	}
}