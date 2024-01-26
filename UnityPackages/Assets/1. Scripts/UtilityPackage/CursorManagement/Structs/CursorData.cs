using System;
using UnityEngine;

namespace Structs.CursorStructs
{
	[Serializable]
	public struct CursorData
	{
		public Texture2D CursorTexture;
		public Vector2 Hotspot;
	}
}