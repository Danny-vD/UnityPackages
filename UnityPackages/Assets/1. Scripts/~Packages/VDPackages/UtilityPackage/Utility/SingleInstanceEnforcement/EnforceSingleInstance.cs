using System.Collections.Generic;
using UnityEngine;
using VDFramework;

namespace VDPackages.UtilityPackage.Utility.SingleInstanceEnforcement
{
	/// <summary>
	/// A utility class used to ensure a group of instances/components are only active once (useful to prevent loading multiple instances between scene reloads)
	/// </summary>
	public class EnsureSingleInstance : BetterMonoBehaviour
	{
		public static List<int> loadedGUIDs = new List<int>();

		[SerializeField, Tooltip("Used to differentiate between different sets of single instances\nEvery ID may only be active once")]
		private int guid;

		[SerializeField, Tooltip("OPTIONAL if null all the children will be set to active")]
		private GameObject singleInstanceParent;

		private bool isValid = false;

		private void Awake()
		{
			if (loadedGUIDs.Contains(guid))
			{
				Destroy(gameObject);
				return;
			}
			
			loadedGUIDs.Add(guid);
			isValid = true;

			if (transform.childCount > 0)
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					transform.GetChild(i).gameObject.SetActive(true);
				}
			}
			else if (singleInstanceParent != null)
			{
				singleInstanceParent.SetActive(true);
			}
		}

		private void OnDestroy()
		{
			if (isValid)
			{
				loadedGUIDs.Remove(guid);
			}
		}
	}
}