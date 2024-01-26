using CursorManagement.Singletons;
using Structs.CursorStructs;
using UnityEngine;

namespace CursorManagement.CursorComponents
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