using System.Collections.Generic;
using CursorManagement.Singletons;
using Structs.CursorStructs;
using UnityEngine;
using Utility.SerializableDictionary;
using UtilityPackage.CursorManagement.CursorUtility;

namespace CursorManagement.CursorComponents
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

		public override bool AreConditionsMet()
		{
			if (keepUsingAfterStoppedMoving && draggingStarted || CursorMovementChecker.Instance.IsCursorMoving)
			{
				foreach (KeyValuePair<MouseButtonUtil.MouseButton, CursorData> pair in draggingData)
				{
					if (MouseButtonHeldChecker.Instance.IsButtonHeld(pair.Key))
					{
						if (!pair.Value.Equals(dataToSet)) // Prevent constantly updating to the same cursor
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