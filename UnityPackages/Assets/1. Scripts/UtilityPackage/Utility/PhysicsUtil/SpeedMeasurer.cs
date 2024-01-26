using UnityEngine;
using UtilityPackage.Attributes;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil
{
	public class SpeedMeasurer : BetterMonoBehaviour
	{
		[SerializeField, ReadOnly]
		private float speed;

		private Rigidbody rigidbdy;

		/// <summary>
		/// Get the current speed of the object in m/s
		/// <para>Will return the rigidbody velocity if one is present</para>
		/// </summary>
		public float Speed => speed;
		
		private Vector3 oldPosition;

		private void Start()
		{
			speed    = 0;
			rigidbdy = GetComponent<Rigidbody>();
		}

		private void LateUpdate()
		{
			if (rigidbdy) // Only calculate speed if no Rigidbody present
			{
				speed = rigidbdy.velocity.magnitude;
				return;
			}
			
			speed = CalculateSpeed();

			oldPosition = CachedTransform.position;
		}

		private float CalculateSpeed()
		{
			Vector3 currentPosition = CachedTransform.transform.position;
			float deltaMovement = Vector3.Distance(currentPosition, oldPosition);

			return deltaMovement / Time.deltaTime;
		}
	}
}