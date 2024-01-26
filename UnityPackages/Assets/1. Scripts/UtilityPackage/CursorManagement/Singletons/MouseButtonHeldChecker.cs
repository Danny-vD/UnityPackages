using System.Collections.Generic;
using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using VDFramework.Extensions;
using VDFramework.Singleton;

namespace UtilityPackage.CursorManagement.Singletons
{
	public class MouseButtonHeldChecker : Singleton<MouseButtonHeldChecker>
	{
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
				downTimePerButton.Add(mouseButton, 0);
			}
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;
			
			foreach (MouseButtonUtil.MouseButton mouseButton in mouseButtons)
			{
				if (MouseButtonUtil.IsButtonPressed(mouseButton))
				{
					downTimePerButton[mouseButton] += deltaTime;
				}
				else
				{
					downTimePerButton[mouseButton] = 0;
				}
			}
		}

		public bool IsButtonHeld(MouseButtonUtil.MouseButton mouseButton)
		{
			return downTimePerButton[mouseButton] >= buttonDownTimeThreshold;
		}
	}
}