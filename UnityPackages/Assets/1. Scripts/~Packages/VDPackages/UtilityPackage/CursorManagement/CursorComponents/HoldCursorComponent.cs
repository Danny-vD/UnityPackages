using System.Collections.Generic;
using UnityEngine;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;
using VDPackages.UtilityPackage.CursorManagement.Singletons;
using VDPackages.UtilityPackage.CursorManagement.Structs;

namespace VDPackages.UtilityPackage.CursorManagement.CursorComponents
{
	public class HoldCursorComponent : AbstractCursorComponent
	{
		[SerializeField, Tooltip("Order matters, higher placed will be shown over lower")]
		private SerializableDictionary<MouseButtonUtil.MouseButton, CursorData> holdingData;

		private CursorData dataToSet;

		public override bool IsAdditiveEffect => false;

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true; // Always update when we are activated
		}

		public override bool AreConditionsMet()
		{
			foreach (KeyValuePair<MouseButtonUtil.MouseButton, CursorData> pair in holdingData)
			{
				if (MouseButtonHeldChecker.Instance.IsButtonHeld(pair.Key))
				{
					if (!pair.Value.Equals(dataToSet)) // Prevent constantly updating to the same cursor
					{
						ShouldUpdateCursor = true;
					}

					dataToSet = pair.Value;
					return true;
				}
			}

			return false;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;

			return dataToSet;
		}
	}
}