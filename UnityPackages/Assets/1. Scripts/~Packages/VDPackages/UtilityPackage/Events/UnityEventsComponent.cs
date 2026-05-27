using UnityEngine;
using UnityEngine.Events;
using VDFramework;

namespace VDPackages.UtilityPackage.Events
{
	/// <summary>
	/// Holds a collection of <see cref="UnityEvent"/> that can be retrieved or invoked
	/// </summary>
	/// <remarks>
	/// This is fantastic in combination with an animator since an animator event is quite limited in what it can invoke, while an UnityEvent has a lot of control
	/// </remarks>
	public class UnityEventsComponent : BetterMonoBehaviour
	{
		[SerializeField]
		private UnityEvent[] events;

		/// <summary>
		/// Get the <see cref="UnityEvent"/> at the specified index
		/// </summary>
		public UnityEvent GetEvent(int index)
		{
			return events[index];
		}

		/// <summary>
		/// Invoke the <see cref="UnityEvent"/> at the specified index
		/// </summary>
		public void InvokeEvent(int index)
		{
			events[index].Invoke();
		}
	}
}