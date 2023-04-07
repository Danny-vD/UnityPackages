using System.Collections;
using System.IO;
using UnityEngine;
using VDFramework;

namespace Utility
{
	public class ScreenshotTaker : BetterMonoBehaviour
	{
		[SerializeField, Tooltip("Pathname to save the screenshot file to.\nPath will only be used in a build")]
		private string pathName = "../Screenshots/Picture.png";

		[SerializeField, Tooltip("Factor by which to increase resolution.")]
		private int superSize = 1;

		[ContextMenu("Take Screenshot")]
		private void TakeScreenshot()
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
			{
				StartCoroutine(CaptureSceneCameraScreenshot());
				return;
			}
#endif

			CaptureScreenshot();
		}

		private void CaptureScreenshot()
		{
			ScreenCapture.CaptureScreenshot(GetFileName(), superSize);
		}

		private string GetFileName()
		{
#if UNITY_EDITOR
			string fileName = pathName;

			fileName = fileName.Replace('\\', '/');
			
			int extensionIndex = fileName.LastIndexOf('.');
			string extension = fileName.Substring(extensionIndex);
			fileName = fileName.Remove(extensionIndex);
			
			int fileNameIndex = fileName.LastIndexOf('/');
			fileName = fileName.Substring(fileNameIndex);
			
			string folderPath = GetFolderPath();
			int fileCount = Directory.GetFiles(folderPath).Length;
			
			folderPath += fileName;
			folderPath += fileCount;
			folderPath += extension;

			return folderPath;
#else
			string fileName = pathName;

			fileName = fileName.Replace('\\', '/');

			int extensionIndex = fileName.LastIndexOf('.');
			string extension = fileName.Substring(extensionIndex);
			fileName = fileName.Remove(extensionIndex);

			int fileNameIndex = fileName.LastIndexOf('/');
			string folderPath = fileName.Remove(fileNameIndex);
			int fileCount = Directory.GetFiles(folderPath).Length;

			fileName += fileCount;
			fileName += extension;

			return fileName;
#endif
		}

#if UNITY_EDITOR
		private IEnumerator CaptureSceneCameraScreenshot()
		{
			Camera mainCamera = Camera.main;
			GameObject activeCamera;
			Vector3 oldPosition = Vector3.zero;
			Quaternion oldRotation = Quaternion.identity;

			bool noActiveCamera = ReferenceEquals(mainCamera, null);

			// If there is an active camera in the scene, move and use that one; else create a new camera and destroy it afterwards
			if (noActiveCamera)
			{
				activeCamera = new GameObject("TempCamera");
				activeCamera.AddComponent<Camera>();
			}
			else
			{
				Transform maincameraTransform = mainCamera.transform;

				activeCamera = mainCamera.gameObject;
				oldPosition  = maincameraTransform.position;
				oldRotation  = maincameraTransform.rotation;
			}

			Transform sceneCameraTransform = UnityEditor.SceneView.lastActiveSceneView.camera.transform;
			activeCamera.transform.position = sceneCameraTransform.position;
			activeCamera.transform.rotation = sceneCameraTransform.rotation;

			// Forces the editor to display the gameview (necessary for screenshot)
			UnityEditor.EditorApplication.ExecuteMenuItem("Window/General/Game");

			try
			{
				CaptureScreenshot();
				yield return new WaitForSeconds(.5f); // Wait for a moment to give enough time to write the image before cleanup
			}
			finally // Make sure we always clean up, even if an error happens when trying to capture
			{
				if (noActiveCamera)
				{
					DestroyImmediate(activeCamera);
				}
				else
				{
					activeCamera.transform.position = oldPosition;
					activeCamera.transform.rotation = oldRotation;
				}
			}
		}

		[UnityEditor.MenuItem("Screenshots/Take Screenshot _F12")]
		private static void TakeScreenshotStatic()
		{
			ScreenshotTaker screenshot = FindObjectOfType<ScreenshotTaker>(true);

			if (screenshot == null)
			{
				Debug.LogWarning($"No screenshot was taken, make sure a {nameof(ScreenshotTaker)} is present in the scene!");
				return;
			}

			screenshot.TakeScreenshot();
		}

		[UnityEditor.MenuItem("Screenshots/Open Screenshot folder #F12")]
		private static void OpenScreenshotFolder()
		{
			System.Diagnostics.Process.Start(GetFolderPath());
		}
		
		private static string GetFolderPath()
		{
			string pathName = Directory.GetCurrentDirectory().Replace('\\', '/');
			int fileNameIndex = pathName.LastIndexOf('/');
			string folderPath = pathName.Remove(fileNameIndex);
			folderPath += "/Screenshots";

			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
			
			return folderPath;
		}
#endif
	}
}