using System;
using UnityEngine;
using VDFramework;

namespace Utility.CopyTargetUtils
{
	/// <summary>
	/// Copies the camera rotation to this transform
	/// </summary>
	public class RotateWithCamera : BetterMonoBehaviour
	{
		[SerializeField]
		private Transform cameraTransform;

		private void Awake()
		{
			if (ReferenceEquals(cameraTransform, null))
			{
				ResetCameraTransform();
			}
		}

		private void ResetCameraTransform()
		{
			Camera main = Camera.main;

			if (ReferenceEquals(main, null))
			{
				Debug.LogError("No camera present in the scene!" + Environment.NewLine + "Destroying this component...", gameObject);
				Destroy(this);
				return;
			}

			cameraTransform = main.transform;
		}

		private void LateUpdate()
		{
			CachedTransform.rotation = cameraTransform.rotation;
		}
	}
}