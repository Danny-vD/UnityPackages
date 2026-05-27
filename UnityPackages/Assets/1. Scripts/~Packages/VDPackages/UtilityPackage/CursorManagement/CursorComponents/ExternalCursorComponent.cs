using VDPackages.UtilityPackage.CursorManagement.Structs;

namespace VDPackages.UtilityPackage.CursorManagement.CursorComponents
{
	/// <summary>
	/// A cursor component that is wholly managed by external factors<br/>
	/// This allows creating custom cursors that don't fit a broader category and is unique per situation
	/// </summary>
	public class ExternalCursorComponent : AbstractCursorComponent
	{
		private CursorData cursorDatum;

		private bool active = false;
		
		public override bool IsAdditiveEffect => false;

		public override bool AreConditionsMet()
		{
			return active;
		}

		protected override void OnActivate()
		{
			ShouldUpdateCursor = true;
		}

		public override CursorData GetCursorData()
		{
			ShouldUpdateCursor = false;
			return cursorDatum;
		}

		public void SetCursorData(CursorData cursorData)
		{
			ShouldUpdateCursor = true;

			cursorDatum = cursorData;
		}

		public void SetActive(bool isActive)
		{
			active = isActive;
		}
	}
}