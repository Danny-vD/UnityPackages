using System;
using UnityEngine;
using VDFramework;

namespace Utility.PhysicsUtil.Callbacks
{
	public class CollisionCallback2D : BetterMonoBehaviour
	{
		public event Action<Collision2D> OnCollisionEntered = delegate { };
		public event Action<Collision2D> OnCollisionExited = delegate { };

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