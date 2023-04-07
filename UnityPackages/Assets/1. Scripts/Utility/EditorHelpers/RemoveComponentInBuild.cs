using VDFramework;

namespace Utility.EditorHelpers
{
	public class RemoveComponentInBuild : BetterMonoBehaviour
	{
		protected virtual bool RemoveObject => false;

#if !UNITY_EDITOR
		protected virtual void Awake()
		{
		}
#else
		protected virtual void Awake()
		{
			if (RemoveObject)
			{
				Destroy(gameObject);
			}
			else
			{
				Destroy(this);
			}
		}
#endif
	}
}