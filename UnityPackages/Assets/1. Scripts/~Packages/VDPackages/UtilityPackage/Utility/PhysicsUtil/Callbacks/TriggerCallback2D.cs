using UnityEngine;
using UnityEngine.Events;
using VDFramework;

namespace VDPackages.UtilityPackage.Utility.PhysicsUtil.Callbacks
{
	public class TriggerCallback2D : BetterMonoBehaviour
	{
		public UnityEvent<Collider2D> OnTriggerEntered;
		public UnityEvent<Collider2D> OnTriggerExited;

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