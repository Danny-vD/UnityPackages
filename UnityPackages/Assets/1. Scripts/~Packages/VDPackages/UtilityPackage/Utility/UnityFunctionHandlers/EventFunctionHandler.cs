using UnityEngine;
using UnityEngine.Events;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.Utility.UnityFunctionHandlers.Enums;

namespace VDPackages.UtilityPackage.Utility.UnityFunctionHandlers
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