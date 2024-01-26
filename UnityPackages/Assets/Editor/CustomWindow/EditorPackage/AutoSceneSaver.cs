using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomWindow.EditorPackage
{
	public class AutoSceneSaver : EditorWindow
	{
		[SerializeField]
		private bool autosave = true;

		[MenuItem("Utility/Autosave")]
		public static void ShowWindow()
		{
			GetWindow<AutoSceneSaver>("Auto Save");
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("KEEP THIS WINDOW OPEN TO AUTO SAVE THE SCENE ON ANY CHANGES");
			autosave = EditorGUILayout.Toggle("AUTOSAVE", autosave);
		}
		
		private void Update()
		{
			if (!autosave)
			{
				return;
			}

			Scene currentScene = SceneManager.GetActiveScene();

			if (currentScene.isDirty)
			{
				EditorSceneManager.SaveScene(currentScene);
			}
		}
	}
}