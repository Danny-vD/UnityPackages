using UnityEngine;
using UtilityPackage.CursorManagement.Singletons;
using UtilityPackage.CursorManagement.Structs;

namespace UtilityPackage.CursorManagement.CursorComponents
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