using JetBrains.Annotations;
using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using VDFramework;

namespace UtilityPackage.Utility.CursorUtil
{
	[DisallowMultipleComponent]
	public class CursorHider : BetterMonoBehaviour
	{
		private enum CursorHideMode
		{
			AlwaysShowCursor,
			HideCursor,
			
			[UsedImplicitly]
			HideCursorAfterClick,
		}

		[SerializeField, Tooltip("When should the cursor be hidden?")]
		private CursorHideMode hideMode = CursorHideMode.HideCursor;

		private void OnEnable()
		{
			if (hideMode == CursorHideMode.AlwaysShowCursor)
			{
				ShowCursor();
			}
			else // HideCursorAfterClick || HideCursor
			{
				MouseButtonUtil.OnAnyMouseButtonUp += HideCursor;
			}

			if (hideMode == CursorHideMode.HideCursor)
			{
				HideCursor();
			}
		}

		private void OnDisable()
		{
			MouseButtonUtil.OnAnyMouseButtonUp -= HideCursor;
		}

		public static void HideCursor()
		{
			Cursor.visible = false;
		}

		public static void ShowCursor()
		{
			Cursor.visible = true;
		}
	}
}