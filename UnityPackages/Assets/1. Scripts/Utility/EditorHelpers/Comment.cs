using UnityEngine;

namespace Utility.EditorHelpers
{
	/// <summary>
	/// A utility class that has a text area to write a comment in
	/// </summary>
	public class Comment : RemoveComponentInBuild
	{
		[SerializeField, TextArea(3, 8)]
		private string comment;
	}
}