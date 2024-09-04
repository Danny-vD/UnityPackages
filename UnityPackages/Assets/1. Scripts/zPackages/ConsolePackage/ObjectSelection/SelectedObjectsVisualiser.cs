using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.UnityExtensions;

namespace ConsolePackage.ObjectSelection
{
	public class SelectedObjectsVisualiser : BetterMonoBehaviour
	{
		[SerializeField]
		private GameObject prefab = null;

		public void Redraw(List<GameObject> objects)
		{
			CachedTransform.DestroyChildren();

			int count = 0;

			foreach (GameObject @object in objects)
			{
				GameObject item = Instantiate(prefab, CachedTransform);
				item.GetComponentInChildren<Text>().text = $"{count}: {@object.name} [{@object.GetInstanceID()}]";

				Button button = item.GetComponentInChildren<Button>();
				button.onClick.AddListener(RemoveObject);

				++count;

				void RemoveObject()
				{
					objects.Remove(@object);
					Destroy(item);
					Redraw(objects);
				}
			}
		}
	}
}