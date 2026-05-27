
using System;
using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM
#else
using System.Collections.Generic;
#endif

namespace VDPackages.ConsolePackage.Console
{
	[Serializable]
	public class ConsoleInputChecker
	{
#if ENABLE_INPUT_SYSTEM
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