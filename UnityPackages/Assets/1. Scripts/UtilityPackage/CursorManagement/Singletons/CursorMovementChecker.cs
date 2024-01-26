using System.Collections;
using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using VDFramework.Singleton;

namespace UtilityPackage.CursorManagement.Singletons
{
	public class CursorMovementChecker : Singleton<CursorMovementChecker>
	{
		[SerializeField, Tooltip("The interval in frames for when it should check whether the cursor moved\nLower values might lead to inaccuracy while higher values might lead to unresponsiveness")]
		private int checkInterval = 30;

		[SerializeField, Tooltip("The minimum distance the cursor has to move to count as 'moving'")] 
		private float movementThreshold = 5;

		public bool IsCursorMoving { get; private set; }

		private Vector2 lastMousePosition;

		private void OnEnable()
		{
			StartCoroutine(CheckMoving());

			lastMousePosition = MouseButtonUtil.MousePosition2D;
		}

		private IEnumerator CheckMoving()
		{
			while (true)
			{
				for (int i = 0; i < checkInterval; i++)
				{
					yield return null;
				}

				Vector2 currentMousePosition = MouseButtonUtil.MousePosition2D;

				Vector2 delta = currentMousePosition - lastMousePosition;
				IsCursorMoving = delta.magnitude >= movementThreshold;

				lastMousePosition = currentMousePosition;
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}
	}
}