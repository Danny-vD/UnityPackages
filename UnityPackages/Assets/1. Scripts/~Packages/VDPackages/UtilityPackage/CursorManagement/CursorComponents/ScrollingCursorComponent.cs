using UnityEngine;
using VDPackages.UtilityPackage.CursorManagement.Singletons;
using VDPackages.UtilityPackage.CursorManagement.Structs;

namespace VDPackages.UtilityPackage.CursorManagement.CursorComponents
{
	public class ScrollingCursorComponent : AbstractCursorComponent
	{
		[SerializeField]
		private CursorData scrollingDatum;
		
		public override bool IsAdditiveEffect => false;

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true;
		}

		public override bool AreConditionsMet()
		{
			return MouseScrollChecker.Instance.IsScrolling;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;
			return scrollingDatum;
		}
	}
}