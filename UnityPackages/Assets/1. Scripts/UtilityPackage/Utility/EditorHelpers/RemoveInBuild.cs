using UnityEngine;
using VDFramework;

namespace Utility.EditorHelpers
{
	/// <summary>
	/// Remove this component or object outside of the Unity Editor
	/// </summary>
	public class RemoveInBuild : BetterMonoBehaviour
	{
#if UNITY_EDITOR
		[SerializeField, Tooltip("If true this object will not be removed when the editor goes into playmode")]
		private bool persistInPlayMode = true;
#endif

		protected virtual bool RemoveObject => false;

		protected virtual void Awake()
		{
#if UNITY_EDITOR
			if (persistInPlayMode)
			{
				return;
			}
#endif

			if (RemoveObject)
			{
				Destroy(gameObject);
			}
			else
			{
				Destroy(this);
			}
		}
	}
}