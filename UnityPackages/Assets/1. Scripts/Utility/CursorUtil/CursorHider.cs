using UnityEngine;
using VDFramework;

namespace Utility.CursorUtil
{
	[DisallowMultipleComponent]
	public class CursorHider : BetterMonoBehaviour
	{
		private enum CursorHideMode
		{
			AlwaysShowCursor,
			HideCursor,
			HideCursorAfterClick,
		}

		[SerializeField, Tooltip("When should the cursor be hidden?")]
		private CursorHideMode hideMode = CursorHideMode.HideCursor;

		private void Start()
		{
			if (hideMode == CursorHideMode.AlwaysShowCursor)
			{
				ShowCursor();
			}
			else
			{
				MouseUtil.OnAnyMouseButtonUp += HideCursor;
			}

			if (hideMode == CursorHideMode.HideCursor)
			{
				HideCursor();
			}
		}

		private void OnEnable()
		{
			if (!MouseUtil.IsInitialized)
			{
				return;
			}
			
			Start();
		}

		private void OnDisable()
		{
			MouseUtil.OnAnyMouseButtonUp -= HideCursor;
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