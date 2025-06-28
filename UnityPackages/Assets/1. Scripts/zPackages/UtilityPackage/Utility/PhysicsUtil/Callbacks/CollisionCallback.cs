using UnityEngine;
using UnityEngine.Events;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil.Callbacks
{
	public class CollisionCallback : BetterMonoBehaviour
	{
		public UnityEvent<Collision> OnCollisionEntered;
		public UnityEvent<Collision> OnCollisionExited;

		private void OnCollisionEnter(Collision other)
		{   
			OnCollisionEntered.Invoke(other);
		}

		private void OnCollisionExit(Collision other)
		{
			OnCollisionExited.Invoke(other);
		}
	}
}