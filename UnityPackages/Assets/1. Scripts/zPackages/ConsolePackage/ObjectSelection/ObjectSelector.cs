using System.Collections.Generic;
using System.Linq;
using ConsolePackage.Console;
using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using UtilityPackage.CursorManagement.CursorUtility.Singletons;
using VDFramework;

namespace ConsolePackage.ObjectSelection
{
	public class ObjectSelector : BetterMonoBehaviour
	{
		[Tooltip("The camera to raycast from (defaults to Camera.Main)")]
		public Camera RaycastFrom;

		[SerializeField]
		private SelectedObjectsVisualiser visualiser = null;

		[SerializeField, Tooltip("Keep in mind that you cannot select Screen space overlay canvases, no matter what")]
		private LayerMask SelectableLayers = default;

		[SerializeField]
		private ObjectSelectorInputChecker inputChecker;

		public List<object> SelectedObjects => selectedObjects.Select(item => item as object).ToList();

		private List<GameObject> selectedObjects;

		private void Awake()
		{
			selectedObjects = new List<GameObject>();

			if (RaycastFrom == null)
			{
				if (Camera.main == null)
				{
					ConsoleManager.LogError("No suitable Camera to raycast from");
					return;
				}

				RaycastFrom = Camera.main;
			}
		}

		private void OnEnable()
		{
			MouseButtonUtil.OnLeftMouseButtonUp += CheckIfSelectedObject;
			inputChecker.OnEnable();
		}

		private void OnDisable()
		{
			MouseButtonUtil.OnLeftMouseButtonUp -= CheckIfSelectedObject;
			inputChecker.OnDisable();
		}

		private void CheckIfSelectedObject()
		{
			if (RaycastFrom == null)
			{
				return;
			}

			if (RayCast(out GameObject hitObject))
			{
				if (selectedObjects.Contains(hitObject))
				{
					RemoveFromSelection(hitObject);
					return;
				}
				
				if (inputChecker.AddToSelectionButtonPressed())
				{
					AddToSelection(hitObject);
				}
				else
				{
					SelectObject(hitObject);
				}
			}
		}

		/// <summary>
		/// Force the objectSelector to loop over the list and check if all objects still exist
		/// </summary>
		public void CheckValid()
		{
			selectedObjects.RemoveAll(item => item == null);
			visualiser.Redraw(selectedObjects);
		}

		public void RemoveFromSelection(GameObject selectedObject)
		{
			selectedObjects.Remove(selectedObject);
			visualiser.Redraw(selectedObjects);
		}

		private void SelectObject(GameObject selectedObject)
		{
			selectedObjects.Clear();
			AddToSelection(selectedObject);
		}

		private void AddToSelection(GameObject selectedObject)
		{
			selectedObjects.Add(selectedObject);
			visualiser.Redraw(selectedObjects);
		}

		private bool RayCast(out GameObject objectHit)
		{
			Ray ray = RaycastFrom.ScreenPointToRay(CursorUtil.MousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, SelectableLayers))
			{
				objectHit = hit.transform.gameObject;
				return true;
			}

			objectHit = null;
			return false;
		}
	}
}