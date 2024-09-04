#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
#else
using System.Collections.Generic;
#endif
using System;
using System.Linq;
using UnityEngine;

namespace ConsolePackage.ObjectSelection
{
	[Serializable]
	public class ObjectSelectorInputChecker
	{
#if UNITY_INPUT_SYSTEM
		[SerializeField]
		private InputActionReference addToSelectionInput;

		public void OnEnable()
		{
			addToSelectionInput.action.Enable();
		}

		public void OnDisable()
		{
			addToSelectionInput.action.Disable();
		}

		private bool CheckInput()
		{
			return addToSelectionInput.action.IsPressed();
		}
#else
		[SerializeField, Tooltip("You need to press at least 1 of these keys to Add to the selection, instead of override it")]
		private List<KeyCode> addToSelectionKeys = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.RightControl };

		private bool CheckInput()
		{
			return addToSelectionKeys.Any(Input.GetKey);
		}

		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}
#endif

		public bool AddToSelectionButtonPressed()
		{
			return CheckInput();
		}
	}
}