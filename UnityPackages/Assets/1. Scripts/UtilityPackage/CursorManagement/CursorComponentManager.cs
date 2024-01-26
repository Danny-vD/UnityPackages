using System.Collections.Generic;
using UnityEngine;
using UtilityPackage.CursorManagement.Structs;
using VDFramework.Singleton;

namespace UtilityPackage.CursorManagement
{
	/// <summary>
	/// Manages CursorComponents in a priority-based order
	/// </summary>
	public class CursorComponentManager : Singleton<CursorComponentManager>
	{
		[SerializeField, Tooltip("Auto: Use hardware cursors on supported platforms.\nForce Software: Force the use of software cursors.")]
		private CursorMode cursorMode = CursorMode.Auto;

		[SerializeField, Tooltip("Order matters, higher placed will be shown over lower")]
		private List<AbstractCursorComponent> cursorComponents;

		private void LateUpdate()
		{
			CheckComponents();
		}

		private void CheckComponents()
		{
			bool allowMoreComponents = true;

			// Go through the components and activate the ones whose conditions are met. If IsAdditiveEffect is false, then no more components will be activated after that one
			foreach (AbstractCursorComponent cursorComponent in cursorComponents)
			{
				// Check if the conditions for this component are met, but only if we can still activate more components
				if (allowMoreComponents && cursorComponent.AreConditionsMet())
				{
					// Check if we already activated this component
					if (!cursorComponent.IsActive)
					{
						cursorComponent.Activate();
					}

					// Check if the component has an update for us
					if (cursorComponent.ShouldUpdateCursor)
					{
						SetCursor(cursorComponent.GetCursorData(), cursorMode);
					}

					allowMoreComponents = cursorComponent.IsAdditiveEffect;
				} // Either it's not allowed to be activated (because something higher in the list prevented it) or the conditions are not met, either way, deactivate it
				else if (cursorComponent.IsActive)
				{
					cursorComponent.Deactivate();
				}
			}
		}

		private static void SetCursor(CursorData data, CursorMode cursorMode = CursorMode.Auto)
		{
			Cursor.SetCursor(data.CursorTexture, data.Hotspot, cursorMode);
		}
	}
}