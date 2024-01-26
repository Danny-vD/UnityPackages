using System;
using UnityEngine;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil.Callbacks
{
	public class CollisionCallback : BetterMonoBehaviour
	{
		public event Action<Collision> OnCollisionEntered = delegate { };
		public event Action<Collision> OnCollisionExited = delegate { };

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