using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using UnityEngine.Events;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace UtilityPackage.Utility.UnityFunctionHandlers
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