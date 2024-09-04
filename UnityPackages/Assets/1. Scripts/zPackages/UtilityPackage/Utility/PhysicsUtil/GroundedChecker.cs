using System.Collections.Generic;
using UnityEngine;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil
{
	/// <summary>
	/// An extremely accurate way to check groundedness of an object by checking the angle of all contact points with a surface *and* checking the angle of the hitNormal from the raycast towards those points
	/// </summary>
	[DisallowMultipleComponent]
	public class GroundedChecker : BetterMonoBehaviour
	{
		[SerializeField, Header("Layers that will never count as grounded when touched")]
		private LayerMask IgnoreLayer = 1 << 5; // UI Layer

		[SerializeField, Header("The maximum allowed slope in degrees for it to still count as grounded"), Range(0, 90)]
		private float slopeThreshold = 30;

		[SerializeField, Tooltip("Above this distance any contact points with a surface will be ignored" +
								 " (useful when OnCollisionExit does not trigger correctly)")]
		private float objectHeight = 1.0f;

		private bool isGrounded;

		/// <summary>
		/// Is any touched surface normal within the grounded angle threshold?
		/// </summary>
		public bool IsGrounded => checkedSinceUpdate ? isGrounded : CheckGrounded();

		/// <summary>
		/// Returns a copy of all the contact points
		/// </summary>
		public List<ContactPoint> ContactPoints => new List<ContactPoint>(contactPoints);

		private readonly List<ContactPoint> contactPoints = new List<ContactPoint>();
		
		private bool checkedSinceUpdate = false;

		private bool CheckGrounded()
		{
			checkedSinceUpdate = true;
			isGrounded         = false;

			// Remove the nulls and any points above a certain distance (in case collission exit was not triggered correctly)
			contactPoints.RemoveAll(point =>
				point.otherCollider == null || point.otherCollider.gameObject == null ||
				Vector3.Distance(CachedTransform.position, point.point) > objectHeight);

			foreach (ContactPoint point in contactPoints)
			{
				// Check the normal of the contact point
				float normalAngle = Vector3.Angle(point.normal, Vector3.up);

				if (normalAngle < slopeThreshold)
				{
					isGrounded = true;
					break;
				}

				// Raycast towards the contact point and check the hit normal (this is not the same as the point.normal)
				Physics.Raycast(point.point, -point.normal, out RaycastHit hit, 0.2f, ~IgnoreLayer);

				float raycastAngle = Vector3.Angle(hit.normal, Vector3.up);

				if (raycastAngle < slopeThreshold)
				{
					isGrounded = true;
					break;
				}
			}

			return isGrounded;
		}

		private void FixedUpdate()
		{
			checkedSinceUpdate = false;
		}

		private void OnCollisionEnter(Collision other)
		{
			if ((1 << other.gameObject.layer & IgnoreLayer) > 0)
			{
				return;
			}

			foreach (ContactPoint contact in other.contacts)
			{
				contactPoints.Add(contact);
			}
		}

		private void OnCollisionStay(Collision other)
		{
			if ((1 << other.gameObject.layer & IgnoreLayer) > 0)
			{
				return;
			}

			GameObject otherObj = other.gameObject; // Cache the object to prevent repeated property access

			// If otherCollider is null, their gameobject is null or we left the collider of the other gameobject, remove all points from them
			contactPoints.RemoveAll(point =>
			{
				GameObject pointObj; // Cache the object to prevent repeated property access

				return point.otherCollider == null || (pointObj = point.otherCollider.gameObject) == null ||
					   pointObj == otherObj;
			});

			foreach (ContactPoint contact in other.contacts)
			{
				contactPoints.Add(contact);
			}
		}

		private void OnCollisionExit(Collision other)
		{
			if ((1 << other.gameObject.layer & IgnoreLayer) > 0)
			{
				return;
			}

			GameObject otherObj = other.gameObject; // Cache the object to prevent repeated property access

			// If otherCollider is null, their gameobject is null or we left the collider of the other gameobject, remove all points from them
			contactPoints.RemoveAll(point =>
			{
				GameObject otherColliderObj; // Cache the object to prevent repeated property access

				return point.otherCollider == null || (otherColliderObj = point.otherCollider.gameObject) == null ||
					   otherColliderObj == otherObj;
			});
		}
	}
}