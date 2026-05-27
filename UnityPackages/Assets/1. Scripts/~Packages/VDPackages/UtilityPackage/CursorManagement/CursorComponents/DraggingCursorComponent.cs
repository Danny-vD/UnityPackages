using System.Collections.Generic;
using UnityEngine;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;
using VDPackages.UtilityPackage.CursorManagement.Singletons;
using VDPackages.UtilityPackage.CursorManagement.Structs;

namespace VDPackages.UtilityPackage.CursorManagement.CursorComponents
{
	public class DraggingCursorComponent : AbstractCursorComponent
	{
		[SerializeField]
		private bool keepUsingAfterStoppedMoving = false;
		
		[SerializeField, Tooltip("Order matters, higher placed will be shown over lower")]
		private SerializableDictionary<MouseButtonUtil.MouseButton, CursorData> draggingData;

		private CursorData dataToSet;

		private bool draggingStarted;

		public override bool IsAdditiveEffect => false;

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true; // Always update when we are activated
		}

		public override bool AreConditionsMet() // Does not use MouseDraggingChecker because doing the check here is more efficient since this is only called when necessary instead of every frame
		{
			if (keepUsingAfterStoppedMoving && draggingStarted || CursorMovementChecker.Instance.IsCursorMoving)
			{
				foreach (KeyValuePair<MouseButtonUtil.MouseButton, CursorData> pair in draggingData)
				{
					if (MouseButtonHeldChecker.Instance.IsButtonHeld(pair.Key))
					{
						if (!pair.Value.Equals(dataToSet)) // Prevent constantly updating to the same cursor by only setting it to true if the result would be different
						{
							ShouldUpdateCursor = true;
						}
						
						dataToSet       = pair.Value;
						draggingStarted = true;
						return true;
					}
				}
			}

			draggingStarted = false;
			
			return false;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;

			return dataToSet;
		}
	}
}