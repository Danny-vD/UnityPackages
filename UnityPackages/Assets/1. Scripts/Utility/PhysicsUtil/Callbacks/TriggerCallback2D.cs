using System;
using UnityEngine;
using VDFramework;

namespace Utility.PhysicsUtil.Callbacks
{
	public class TriggerCallback2D : BetterMonoBehaviour
	{
		public event Action<Collider2D> OnTriggerEntered = delegate { };
		public event Action<Collider2D> OnTriggerExited = delegate { };

		private void OnTriggerEnter2D(Collider2D other)
		{
			OnTriggerEntered.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			OnTriggerExited.Invoke(other);
		}
	}
}