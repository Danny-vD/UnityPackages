using System;
using UnityEngine;

namespace UtilityPackage.CursorManagement.Structs
{
	[Serializable]
	public struct CursorData : IEquatable<CursorData>
	{
		public Texture2D CursorTexture;
		public Vector2 Hotspot;

		public bool Equals(CursorData other) => Equals(CursorTexture, other.CursorTexture) && Hotspot.Equals(other.Hotspot);

		public override bool Equals(object obj) => obj is CursorData other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(CursorTexture, Hotspot);
	}
}