using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.UnityFunctionHandlers
{
	public class EventFunctionHandler : AbstractFunctionHandler
	{
		[SerializeField]
		private SerializableDictionary<UnityFunction, UnityEvent> eventPerFunction;

		protected override void ReactToEvent(UnityFunction unityFunction)
		{
			if (eventPerFunction.TryGetValue(unityFunction, out UnityEvent unityEvent))
			{
				unityEvent.Invoke();
			}
		}
	}
}