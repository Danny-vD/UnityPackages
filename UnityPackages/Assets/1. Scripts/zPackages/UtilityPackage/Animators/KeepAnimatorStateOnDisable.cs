using UnityEngine;
using VDFramework;

namespace UtilityPackage.Animators
{
	public class KeepAnimatorStateOnDisable : BetterMonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Animator>().keepAnimatorStateOnDisable = true;
		}
	}
}