using System;
using UnityEngine;
using VDFramework;

namespace UtilityPackage.Utility.CopyTargetUtils
{
	public class CopyTransformOfTarget : BetterMonoBehaviour
	{
		[Flags]
		private enum Axis
		{
			X = 1,
			Y = 2,
			Z = 4,

			// These are for serialization purposes, to be more descriptive in the inspector
			XY = 3,
			XZ = 5,
			YZ = 6,
		}

		[SerializeField, Tooltip("The target whose position/rotation will be copied")]
		private Transform target;

		[Header("Position")]
		[SerializeField]
		private Axis copyCoordinates = 0;

		[SerializeField, Tooltip("Copy the local position instead of the global (world) coordinates")]
		private bool copyLocalPosition = false;

		[SerializeField, Tooltip("Set the local position instead of the global (world) coordinates")]
		private bool setLocalPosition = true;

		[Space]
		[SerializeField, Tooltip("Relative to world origin")]
		private Vector3 globalPositionOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to the parent")]
		private Vector3 parentPositionOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to the target")]
		private Vector3 targetPositionOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to self")]
		private Vector3 localPositionOffset = Vector3.zero;

		[Header("Rotation")]
		[SerializeField]
		private Axis copyAngles = 0;

		[SerializeField, Tooltip("Copy the local rotation instead of the global (world) rotation")]
		private bool copyLocalRotation = false;

		[SerializeField, Tooltip("Set the local rotation isntead of the global (world) rotation")]
		private bool setLocalRotation = true;

		[Space]
		[SerializeField, Tooltip("Relative to the world origin")]
		private Vector3 globalRotationOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to the parent")]
		private Vector3 parentRotationOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to the target")]
		private Vector3 targetRotationOffset = Vector3.zero;

		[SerializeField, Tooltip("Relative to self")]
		private Vector3 localRotationOffset = Vector3.zero;

		[Header("Scale")]
		[SerializeField]
		private Axis copyScale = 0;

		[SerializeField]
		private Vector3 scaleOffset = Vector3.zero;

		private void Awake()
		{
			if (ReferenceEquals(target, null))
			{
				Debug.LogError("Target is not set", this);
				return;
			}

			if (ReferenceEquals(target, CachedTransform))
			{
				Debug.LogWarning("Copying self may not be desired behaviour", this);
			}
		}

		private void LateUpdate()
		{
			if (target == null)
			{
				return;
			}

			if (copyAngles != 0)
			{
				CopyRotation();

				ApplyRotationOffset();
			}

			if (copyCoordinates != 0)
			{
				CopyPosition();

				ApplyPositionOffset();
			}

			if (copyScale != 0)
			{
				CopyScale();
			}
		}

		private void CopyPosition()
		{
			Vector3 currentPosition = setLocalPosition ? CachedTransform.localPosition : CachedTransform.position;

			Vector3 targetPosition = copyLocalPosition ? target.localPosition : target.position;

			if (copyCoordinates.HasFlag(Axis.X))
			{
				currentPosition.x = targetPosition.x;
			}

			if (copyCoordinates.HasFlag(Axis.Y))
			{
				currentPosition.y = targetPosition.y;
			}

			if (copyCoordinates.HasFlag(Axis.Z))
			{
				currentPosition.z = targetPosition.z;
			}

			if (setLocalPosition)
			{
				CachedTransform.localPosition = currentPosition;
			}
			else
			{
				CachedTransform.position = currentPosition;
			}
		}

		private void ApplyPositionOffset()
		{
			Transform parent = CachedTransform.parent;
			Vector3 offset = globalPositionOffset;

			if (parent)
			{
				offset += parent.right * parentPositionOffset.x;
				offset += parent.up * parentPositionOffset.y;
				offset += parent.forward * parentPositionOffset.z;
			}

			offset += target.right * targetPositionOffset.x;
			offset += target.up * targetPositionOffset.y;
			offset += target.forward * targetPositionOffset.z;

			offset += CachedTransform.right * localPositionOffset.x;
			offset += CachedTransform.up * localPositionOffset.y;
			offset += CachedTransform.forward * localPositionOffset.z;

			if (setLocalPosition)
			{
				offset = transform.InverseTransformVector(offset);
			}

			// Prevent adding an offset to an axis we didn't copy (which would cause infinite translation)
			if (!copyCoordinates.HasFlag(Axis.X))
			{
				offset.x = 0;
			}

			if (!copyCoordinates.HasFlag(Axis.Y))
			{
				offset.y = 0;
			}

			if (!copyCoordinates.HasFlag(Axis.Z))
			{
				offset.z = 0;
			}

			if (setLocalPosition)
			{
				CachedTransform.localPosition += offset;
			}
			else
			{
				CachedTransform.position += offset;
			}
		}

		private void CopyRotation()
		{
			Vector3 currentRotation = setLocalRotation ? CachedTransform.localEulerAngles : CachedTransform.eulerAngles;

			Vector3 targetRotation = copyLocalRotation ? target.localEulerAngles : target.eulerAngles;

			if (copyAngles.HasFlag(Axis.Z))
			{
				currentRotation.z = targetRotation.z;
			}

			if (copyAngles.HasFlag(Axis.X))
			{
				currentRotation.x = targetRotation.x;
			}

			if (copyAngles.HasFlag(Axis.Y))
			{
				currentRotation.y = targetRotation.y;
			}

			if (setLocalPosition)
			{
				CachedTransform.localEulerAngles = currentRotation;
			}
			else
			{
				CachedTransform.eulerAngles = currentRotation;
			}
		}

		private void ApplyRotationOffset()
		{
			Transform parent = CachedTransform.parent;
			Vector3 offset = globalRotationOffset;

			if (parent)
			{
				offset += parent.right * parentRotationOffset.x;
				offset += parent.up * parentRotationOffset.y;
				offset += parent.forward * parentRotationOffset.z;
			}

			offset += target.right * targetRotationOffset.x;
			offset += target.up * targetRotationOffset.y;
			offset += target.forward * targetRotationOffset.z;

			offset += CachedTransform.right * localRotationOffset.x;
			offset += CachedTransform.up * localRotationOffset.y;
			offset += CachedTransform.forward * localRotationOffset.z;

			if (setLocalRotation)
			{
				offset = transform.InverseTransformVector(offset);
			}

			// Prevent adding an offset to an axis we didn't copy (which would cause infinite rotation)
			if (!copyAngles.HasFlag(Axis.X))
			{
				offset.x = 0;
			}

			if (!copyAngles.HasFlag(Axis.Y))
			{
				offset.y = 0;
			}

			if (!copyAngles.HasFlag(Axis.Z))
			{
				offset.z = 0;
			}

			if (setLocalRotation)
			{
				CachedTransform.localEulerAngles += offset;
			}
			else
			{
				CachedTransform.eulerAngles += offset;
			}
		}

		private void CopyScale()
		{
			Vector3 currentScale = CachedTransform.localScale;

			Vector3 targetScale = target.localScale;

			if (copyScale.HasFlag(Axis.X))
			{
				currentScale.x = targetScale.x;

				currentScale.x += scaleOffset.x;
			}

			if (copyScale.HasFlag(Axis.Y))
			{
				currentScale.y = targetScale.y;

				currentScale.y += scaleOffset.y;
			}

			if (copyScale.HasFlag(Axis.Z))
			{
				currentScale.z = targetScale.z;

				currentScale.z += scaleOffset.z;
			}

			CachedTransform.localScale = currentScale;
		}
	}
}