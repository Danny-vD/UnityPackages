using GridPackage.Grid.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Utility.EditorUtils;

namespace CustomInspector.Grid
{
	[CustomEditor(typeof(GridCreator))]
	public class GridCreatorEditor : UnityEditor.Editor
	{
		private GridCreator gridCreator;

		private void OnEnable()
		{
			gridCreator = (GridCreator)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawGenerateButton();

			DrawDefaultInspector();

			EditorGUILayout.Space();
			DrawSeperatorLine();
			EditorGUILayout.Space();

			DrawDeleteButton();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawGenerateButton()
		{
			if (!GUILayout.Button("Generate Grid", EditorStyles.miniButtonMid)) return;

			gridCreator.GenerateGrid();

			if (!EditorApplication.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}

		private void DrawDeleteButton()
		{
			if (!GUILayout.Button("Remove Grid", EditorStyles.miniButtonMid)) return;

			gridCreator.DestroyGrid();

			if (!EditorApplication.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}
}