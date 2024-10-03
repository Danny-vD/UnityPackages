using UnityEngine;
using UtilityPackage.CursorManagement.Structs;

namespace UtilityPackage.CursorManagement.CursorComponents
{
	/// <summary>
	/// Will always pass the <see cref="AreConditionsMet"/> check<br/>
	/// Use as a 'base' option of what the cursor should look like
	/// </summary>
	public class BaseCursorComponent : AbstractCursorComponent
	{
		[SerializeField]
		private CursorData cursorDatum;
		
		public override bool IsAdditiveEffect => false;

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true;
		}

		public override bool AreConditionsMet()
		{
			return true;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;
			return cursorDatum;
		}
	}
}