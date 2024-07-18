using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomWindow.UtilityPackage
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
			EditorGUILayout.LabelField("KEEP THIS WINDOW OPEN ANYWHERE TO AUTOSAVE THE SCENE ON ANY CHANGES");
			autosave = EditorGUILayout.Toggle("ENABLE AUTOSAVE", autosave);
		}

		private void Awake()
		{
			EditorSceneManager.sceneDirtied += SaveSceneIfDirty;
		}

		private void OnDestroy()
		{
			EditorSceneManager.sceneDirtied -= SaveSceneIfDirty;
		}

		private void SaveSceneIfDirty(Scene scene)
		{
			SaveSceneIfDirty();
		}
		
		private void SaveSceneIfDirty()
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