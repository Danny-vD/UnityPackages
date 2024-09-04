using UnityEngine;
using UnityEngine.EventSystems;
using VDFramework.Singleton;

#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

#else
using System.Reflection;
#endif

namespace UtilityPackage.CursorManagement.CursorUtility.Singletons
{
	public class CursorUtil : Singleton<CursorUtil>
	{
#if UNITY_INPUT_SYSTEM
		private InputSystemUIInputModule inputModule;

		private void OnEnable()
		{
			if (inputModule == null)
			{
				inputModule = FindObjectOfType<InputSystemUIInputModule>(false);

				if (inputModule == null)
				{
					Debug.LogErrorFormat("No {0} found in the scene!", nameof(InputSystemUIInputModule));
				}
			}
		}

		public bool TryGetHoveredGameObject(int pointerID, out GameObject hoveredGameObject)
		{
			if (inputModule.IsPointerOverGameObject(pointerID))
			{
				RaycastResult raycastResult = inputModule.GetLastRaycastResult(pointerID);

				if (raycastResult.isValid)
				{
					hoveredGameObject = raycastResult.gameObject;
					return true;
				}
			}

			hoveredGameObject = null;
			return false;
		}

		public bool TryGetHoveredGameObject(out GameObject hoveredGameObject)
		{
			return TryGetHoveredGameObject(Mouse.current.deviceId, out hoveredGameObject);
		}
#else
		private static EventSystem EventSystem => EventSystem.current;
		
		/// <inheritdoc cref="UnityEngine.EventSystems.PointerInputModule.GetPointerData"/>
		private MethodInfo getPointerData; // protected bool GetPointerData(int id, out PointerEventData data, bool create)

		private readonly object[] parameters = new object[] { PointerInputModule.kMouseLeftId, null, false };

		protected override void Awake()
		{
			base.Awake();
			
			getPointerData = typeof(PointerInputModule).GetMethod("GetPointerData", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public bool TryGetHoveredGameObject(int pointerID, out GameObject hoveredGameObject)
		{
			PointerInputModule pointerInputModule = EventSystem.currentInputModule as PointerInputModule;

			if (pointerInputModule != null && EventSystem.IsPointerOverGameObject(pointerID))
			{
				parameters[0] = pointerID;

				getPointerData.Invoke(pointerInputModule, parameters); // The returned value is not relevant (it's always false)

				PointerEventData pointerEventData = (PointerEventData)parameters[1]; 
				
				if (pointerEventData != null)
				{
					hoveredGameObject = pointerEventData.pointerCurrentRaycast.gameObject;
					return true;
				}
			}

			hoveredGameObject = null;
			return false;
		}

		public bool TryGetHoveredGameObject(out GameObject hoveredGameObject)
		{
			return TryGetHoveredGameObject(PointerInputModule.kMouseLeftId, out hoveredGameObject);
		}
#endif
	}
}