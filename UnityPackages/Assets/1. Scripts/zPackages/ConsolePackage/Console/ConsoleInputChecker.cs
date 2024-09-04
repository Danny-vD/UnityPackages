#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;

#else
using System.Collections.Generic;
#endif
using System;
using UnityEngine;

namespace ConsolePackage.Console
{
	[Serializable]
	public class ConsoleInputChecker
	{
#if UNITY_INPUT_SYSTEM
		[SerializeField]
		private InputActionReference openConsoleInput;

		public void OnEnable()
		{
			openConsoleInput.action.Enable();
		}

		public void OnDisable()
		{
			openConsoleInput.action.Disable();
		}

		private bool CheckInput()
		{
			return openConsoleInput.action.IsPressed();
		}
#else
		[Tooltip("The combination of buttons to press to toggle the console")]
		public List<KeyCode> KeysToPress = new List<KeyCode>() { KeyCode.Home };

		private bool CheckInput()
		{
			return KeysToPress.TrueForAll(Input.GetKey);
		}

		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}
#endif

		public bool OpenConsoleKeysPressed()
		{
			return CheckInput();
		}
	}
}