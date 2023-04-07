using System;
using UnityEngine;
using VDFramework;

namespace Utility
{
    public class RotateWithCamera : BetterMonoBehaviour
    {
        private Transform cameraTransform;

        private void Awake()
        {
            ResetCameraTransform();
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
