using System;
using System.Collections;
using UnityEngine;
using VDFramework.Singleton;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility.Singletons;

namespace VDPackages.UtilityPackage.CursorManagement.Singletons
{
	[DefaultExecutionOrder(-5)]
	public class CursorMovementChecker : Singleton<CursorMovementChecker>
	{
		public static event Action OnStartedMoving = delegate { };
		public static event Action OnStoppedMoving = delegate { };
		
		[SerializeField, Tooltip("The interval in frames for when it should check whether the cursor moved\nLower values might lead to inaccuracy while higher values might lead to unresponsiveness")]
		private int checkInterval = 30;

		[SerializeField, Tooltip("The minimum distance the cursor has to move to count as 'moving'")] 
		private float movementThreshold = 5;

		public bool IsCursorMoving { get; private set; }

		private Vector2 lastMousePosition;

		private void OnEnable()
		{
			StartCoroutine(CheckMoving());

			lastMousePosition = CursorUtil.CursorPosition2D;
		}

		private IEnumerator CheckMoving()
		{
			while (true)
			{
				for (int i = 0; i < checkInterval; i++)
				{
					yield return null;
				}

				Vector2 currentMousePosition = CursorUtil.CursorPosition2D;

				Vector2 delta = currentMousePosition - lastMousePosition;
				bool wasCursorMoving = IsCursorMoving;
				IsCursorMoving = delta.magnitude >= movementThreshold;

				switch (IsCursorMoving)
				{
					case true when !wasCursorMoving:
						OnStartedMoving.Invoke();
						break;
					case false when wasCursorMoving:
						OnStoppedMoving.Invoke();
						break;
				}

				lastMousePosition = currentMousePosition;
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}
	}
}