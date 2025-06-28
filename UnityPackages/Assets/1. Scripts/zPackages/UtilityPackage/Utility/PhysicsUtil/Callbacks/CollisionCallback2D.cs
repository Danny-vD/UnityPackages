using UnityEngine;
using UnityEngine.Events;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil.Callbacks
{
	public class CollisionCallback2D : BetterMonoBehaviour
	{
		public UnityEvent<Collision2D> OnCollisionEntered;
		public UnityEvent<Collision2D> OnCollisionExited;

		private void OnCollisionEnter2D(Collision2D other)
		{
			OnCollisionEntered.Invoke(other);
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			OnCollisionExited.Invoke(other);
		}
	}
}