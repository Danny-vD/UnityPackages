using System;
using System.Collections.Generic;
using UnityEngine;
using VDFramework.Extensions;
using VDFramework.Singleton;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;

namespace VDPackages.UtilityPackage.CursorManagement.Singletons
{
	[DefaultExecutionOrder(-5)]
	public class MouseButtonHeldChecker : Singleton<MouseButtonHeldChecker>
	{
		public static readonly Dictionary<MouseButtonUtil.MouseButton, Action> OnStartedHoldingButtonEvents = new Dictionary<MouseButtonUtil.MouseButton, Action>();
		public static readonly Dictionary<MouseButtonUtil.MouseButton, Action> OnStoppedHoldingButtonEvents = new Dictionary<MouseButtonUtil.MouseButton, Action>();
		
		[SerializeField, Tooltip("How long (in seconds) a button should be held down for it to count as 'holding'")]
		private float buttonDownTimeThreshold = 0.1f;

		private Dictionary<MouseButtonUtil.MouseButton, float> downTimePerButton;

		private IEnumerable<MouseButtonUtil.MouseButton> mouseButtons;

		protected override void Awake()
		{
			base.Awake();

			downTimePerButton = new Dictionary<MouseButtonUtil.MouseButton, float>();

			mouseButtons = default(MouseButtonUtil.MouseButton).GetValues();

			foreach (MouseButtonUtil.MouseButton mouseButton in mouseButtons)
			{
				OnStartedHoldingButtonEvents.Add(mouseButton, delegate { });
				OnStoppedHoldingButtonEvents.Add(mouseButton, delegate { });
				
				downTimePerButton.Add(mouseButton, 0);
			}
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;
			
			foreach (MouseButtonUtil.MouseButton mouseButton in mouseButtons)
			{
				bool buttonWasHeld = IsButtonHeld(mouseButton);
				
				if (MouseButtonUtil.IsButtonPressed(mouseButton))
				{
					downTimePerButton[mouseButton] += deltaTime;

					if (!buttonWasHeld && IsButtonHeld(mouseButton))
					{
						OnStartedHoldingButtonEvents[mouseButton].Invoke();
					}
				}
				else
				{
					downTimePerButton[mouseButton] = 0;

					if (buttonWasHeld)
					{
						OnStoppedHoldingButtonEvents[mouseButton].Invoke();
					}
				}
			}
		}

		public bool IsButtonHeld(MouseButtonUtil.MouseButton mouseButton)
		{
			return downTimePerButton[mouseButton] >= buttonDownTimeThreshold;
		}
	}
}