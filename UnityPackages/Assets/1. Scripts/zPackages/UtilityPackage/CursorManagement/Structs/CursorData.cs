using System;
using UnityEngine;

namespace UtilityPackage.CursorManagement.Structs
{
	[Serializable]
	public struct CursorData
	{
		public Texture2D CursorTexture;
		public Vector2 Hotspot;
	}
}