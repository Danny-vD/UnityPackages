using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VDFramework.Extensions;
using VDFramework.Singleton;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;

namespace VDPackages.UtilityPackage.CursorManagement.Singletons
{
	[DefaultExecutionOrder(-4)] // -5 Is usually used for all the Cursor/Mouse checkers, but this one depends on MouseButtonHeldChecker so has to happen after that
	public class MouseDraggingChecker : Singleton<MouseDraggingChecker>
	{
		public static readonly Dictionary<MouseButtonUtil.MouseButton, Action> OnStartedDraggingButtonEvents = new Dictionary<MouseButtonUtil.MouseButton, Action>();
		public static readonly Dictionary<MouseButtonUtil.MouseButton, Action> OnStoppedDraggingButtonEvents = new Dictionary<MouseButtonUtil.MouseButton, Action>();

		private static readonly Dictionary<MouseButtonUtil.MouseButton, bool> isDraggingButtonDictionary = new Dictionary<MouseButtonUtil.MouseButton, bool>();

		protected override void Awake()
		{
			base.Awake();

			foreach (MouseButtonUtil.MouseButton mouseButton in default(MouseButtonUtil.MouseButton).GetValues())
			{
				OnStartedDraggingButtonEvents.Add(mouseButton, delegate { });
				OnStoppedDraggingButtonEvents.Add(mouseButton, delegate { });
			}
		}

		private void LateUpdate()
		{
			bool isCursormoving = CursorMovementChecker.Instance.IsCursorMoving;
			
			foreach (MouseButtonUtil.MouseButton mouseButton in default(MouseButtonUtil.MouseButton).GetValues())
			{
				bool wasDragging = IsDragging(mouseButton);

				bool isDragging = isCursormoving && MouseButtonHeldChecker.Instance.IsButtonHeld(mouseButton);
				
				switch (isDragging)
				{
					case true when !wasDragging:
						isDraggingButtonDictionary[mouseButton] = true;
						OnStartedDraggingButtonEvents[mouseButton].Invoke();
						break;
					case false when wasDragging:
						isDraggingButtonDictionary[mouseButton] = false;
						OnStoppedDraggingButtonEvents[mouseButton].Invoke();
						break;
				}
			}
		}

		public static bool IsDragging(MouseButtonUtil.MouseButton mouseButton)
		{
			ForceInitialize(); // Ensure that there is always an instance (otherwise the value would never update)
			
			return isDraggingButtonDictionary[mouseButton];
		}

		public static bool IsAnyButtonDragging()
		{
			ForceInitialize(); // Ensure that there is always an instance (otherwise the value would never update)
			
			return isDraggingButtonDictionary.Any(pair => pair.Value);
		}
	}
}