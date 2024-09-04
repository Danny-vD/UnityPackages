using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using UtilityPackage.CursorManagement.Structs;

namespace UtilityPackage.CursorManagement.CursorComponents
{
	public class ClickCursorComponent : AbstractCursorComponent
	{
		[SerializeField, Tooltip("Order matters, higher placed will be shown over lower")]
		private SerializableDictionary<MouseButtonUtil.MouseButton, CursorData> mouseClickData;
		
		public override bool IsAdditiveEffect => false;

		private MouseButtonUtil.MouseButton lastButtonPressed = (MouseButtonUtil.MouseButton)(-1);
		private CursorData dataToSet;

		protected override void OnDeactivate()
		{
			lastButtonPressed = (MouseButtonUtil.MouseButton)(-1);
		}

		public override bool AreConditionsMet()
		{
			foreach ((MouseButtonUtil.MouseButton button, CursorData data) in mouseClickData)
			{
				if (MouseButtonUtil.IsButtonPressed(button))
				{
					if (lastButtonPressed != button) // Prevent constantly setting the current state to itself
					{
						ShouldUpdateCursor = true;
					}

					lastButtonPressed = button;
					dataToSet         = data;
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