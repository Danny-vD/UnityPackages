using UnityEngine;

namespace UtilityPackage.Utility.EditorHelpers
{
	/// <summary>
	/// Ensures this gameobject will always be selected when clicking on one of the children in the scene
	/// </summary>
	[SelectionBase]
	public class SelectionBase : RemoveInBuild
	{
		protected override bool RemoveObject => true;
	}
}