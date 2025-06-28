using UnityEngine;
using UnityEngine.Events;
using VDFramework;

namespace UtilityPackage.Utility.PhysicsUtil.Callbacks
{
	public class TriggerCallback : BetterMonoBehaviour
	{
		public UnityEvent<Collider> OnTriggerEntered;
		public UnityEvent<Collider> OnTriggerExited;

		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEntered.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnTriggerExited.Invoke(other);
		}
	}
}