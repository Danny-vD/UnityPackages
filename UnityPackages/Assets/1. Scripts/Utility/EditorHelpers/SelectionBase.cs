using UnityEngine;

namespace Utility.EditorHelpers
{
	[SelectionBase]
	public class SelectionBase : RemoveComponentInBuild
	{
		protected override bool RemoveObject => true;
	}
}