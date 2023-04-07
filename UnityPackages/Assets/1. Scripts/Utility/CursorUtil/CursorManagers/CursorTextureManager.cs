using System;
using UnityEngine;
using MouseButton = Utility.CursorUtil.MouseUtil.MouseButton;

namespace Utility.CursorUtil.CursorManagers
{
	public class CursorTextureManager : AbstractCursorManager<CursorTextureManager.CursorData>
	{
		[Serializable]
		public struct CursorData
		{
			public Texture2D cursorTexture;
			public Vector2 Hotspot;
		}

		private void Awake()
		{
			SetCursor(idleDatum);
		}

		protected override void OnStateChanged(MouseButton newState, CursorData datum, bool idle = false)
		{
			SetCursor(datum);
		}

		private static void SetCursor(CursorData data)
		{
			Cursor.SetCursor(data.cursorTexture, data.Hotspot, CursorMode.Auto);
		}
	}
}