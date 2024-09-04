using UnityEngine;
using UtilityPackage.CursorManagement.Structs;
using VDFramework;

namespace UtilityPackage.CursorManagement.CursorUtility
{
	/// <summary>
	/// Script used by the CursorComponents to set a specific CursorData for this gameobject
	/// </summary>
	public class CursorTextureComponent : BetterMonoBehaviour
	{
		[field: SerializeField]
		public CursorData CursorData { get; private set; }
	}
}