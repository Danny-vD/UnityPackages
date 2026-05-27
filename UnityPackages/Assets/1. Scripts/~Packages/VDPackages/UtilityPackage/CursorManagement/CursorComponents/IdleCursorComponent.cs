using UnityEngine;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;
using VDPackages.UtilityPackage.CursorManagement.Structs;

namespace VDPackages.UtilityPackage.CursorManagement.CursorComponents
{
	public class IdleCursorComponent : AbstractCursorComponent
	{
		[SerializeField]
		private CursorData idleDatum;
		
		public override bool IsAdditiveEffect => false;

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true;
		}

		public override bool AreConditionsMet()
		{
			return !MouseButtonUtil.IsAnyMouseButtonDown;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;
			return idleDatum;
		}
	}
}