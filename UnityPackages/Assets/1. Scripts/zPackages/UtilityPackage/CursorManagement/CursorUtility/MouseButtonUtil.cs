using System;
using System.Linq;
using UnityEngine;
using VDFramework.Singleton;
#if UNITY_INPUT_SYSTEM
using Mouse = UnityEngine.InputSystem.Mouse;
#endif

namespace UtilityPackage.CursorManagement.CursorUtility
{
	[DisallowMultipleComponent, DefaultExecutionOrder(-1)] // Default execution order makes sure it runs before anything else (this is great for the query functions since it prevents it being a frame off)
	public class MouseButtonUtil : Singleton<MouseButtonUtil>
	{
		#region Nested Types

#if UNITY_INPUT_SYSTEM
		// Necessary for convenience of other scripts trying to interact with MouseUtil

		/// <summary>
		/// Button indices for <see cref="UnityEngine.InputSystem.LowLevel.MouseState.buttons"/>.
		/// </summary>
		/// <seealso cref="UnityEngine.InputSystem.LowLevel.MouseButton"/>
		public enum MouseButton
		{
			/// <summary>
			/// Left mouse button.
			/// </summary>
			/// <seealso cref="Mouse.leftButton"/>
			Left = UnityEngine.InputSystem.LowLevel.MouseButton.Left,

			/// <summary>
			/// Right mouse button.
			/// </summary>
			/// <seealso cref="Mouse.rightButton"/>
			Right = UnityEngine.InputSystem.LowLevel.MouseButton.Right,

			/// <summary>
			/// Middle mouse button.
			/// </summary>
			/// <seealso cref="Mouse.middleButton"/>
			Middle = UnityEngine.InputSystem.LowLevel.MouseButton.Middle,

			/// <summary>
			/// Second side button.
			/// </summary>
			/// <seealso cref="Mouse.forwardButton"/>
			Forward = UnityEngine.InputSystem.LowLevel.MouseButton.Forward,

			/// <summary>
			/// First side button.
			/// </summary>
			/// <seealso cref="Mouse.backButton"/>
			Back = UnityEngine.InputSystem.LowLevel.MouseButton.Back,
		}
#else
		/// <summary>
		/// Button indices for <see cref="UnityEngine.Input.GetMouseButton"/>
		/// </summary>
		/// <seealso cref="UnityEngine.Input.GetMouseButtonDown"/>
		/// <seealso cref="UnityEngine.Input.GetMouseButtonUp"/>
		public enum MouseButton
		{
			/// <summary>
			/// Left mouse button.
			/// </summary>
			/// <seealso cref="UnityEngine.KeyCode.Mouse0"/>
			Left,

			/// <summary>
			/// Right mouse button.
			/// </summary>
			/// <seealso cref="UnityEngine.KeyCode.Mouse1"/>
			Right,

			/// <summary>
			/// Middle mouse button.
			/// </summary>
			/// <seealso cref="UnityEngine.KeyCode.Mouse2"/>
			Middle,

			/// <summary>
			/// First side button.
			/// </summary>
			/// <seealso cref="UnityEngine.KeyCode.Mouse3"/>
			Back,

			/// <summary>
			/// Second side button.
			/// </summary>
			/// <seealso cref="UnityEngine.KeyCode.Mouse4"/>
			Forward,
		}
#endif

		/// <summary>
		/// A utility class that watches the Button ups and downs and calls the respective events
		/// </summary>
		private class MouseInputEventHandler
		{
			public event Action OnButtonDown = InvokeAnyButtonDown;
			public event Action OnButtonUp = InvokeAnyButtonUp;

			private readonly Func<bool> checkButtonDown;
			private readonly Func<bool> checkButtonUp;

			public bool ButtonPressed { get; private set; }

			public MouseInputEventHandler(Func<bool> buttonDown, Func<bool> buttonUp)
			{
				checkButtonDown = buttonDown;
				checkButtonUp   = buttonUp;
			}

			/// <summary>
			/// Same as Unity's own Update function, but has to be manually called
			/// </summary>
			public void Update()
			{
				if (checkButtonDown())
				{
					ButtonPressed = true;
					OnButtonDown.Invoke();
				}
				else if (checkButtonUp())
				{
					ButtonPressed = false;
					OnButtonUp.Invoke();
				}
			}
		}

		#endregion

		#region Button Listener Events

		/// <summary>
		/// A simplified way to to add/remove listeners to AnyMouseButtonDown
		/// </summary>
		/// <seealso cref="AddAnyButtonDownListener"/>
		/// <seealso cref="RemoveAnyButtonDownListener"/>
		public static event Action OnAnyMouseButtonDown
		{
			add => AddAnyButtonDownListener(value);
			remove => RemoveAnyButtonDownListener(value);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to AnyMouseButtonUp
		/// </summary>
		/// <seealso cref="AddAnyButtonUpListener"/>
		/// <seealso cref="RemoveAnyButtonUpListener"/>
		public static event Action OnAnyMouseButtonUp
		{
			add => AddAnyButtonUpListener(value);
			remove => RemoveAnyButtonUpListener(value);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to LeftMouseButtonDown
		/// </summary>
		/// <seealso cref="AddButtonDownListener"/>
		/// <seealso cref="RemoveButtonDownListener"/>
		public static event Action OnLeftMouseButtonDown
		{
			add => AddButtonDownListener(value);
			remove => RemoveButtonDownListener(value);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to LeftMouseButtonUp
		/// </summary>
		/// <seealso cref="AddButtonUpListener"/>
		/// <seealso cref="RemoveButtonUpListener"/>
		public static event Action OnLeftMouseButtonUp
		{
			add => AddButtonUpListener(value);
			remove => RemoveButtonUpListener(value);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to RightMouseButtonDown
		/// </summary>
		/// <seealso cref="AddButtonDownListener"/>
		/// <seealso cref="RemoveButtonDownListener"/>
		public static event Action OnRightMouseButtonDown
		{
			add => AddButtonDownListener(value, MouseButton.Right);
			remove => RemoveButtonDownListener(value, MouseButton.Right);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to RightMouseButtonUp
		/// </summary>
		/// <seealso cref="AddButtonUpListener"/>
		/// <seealso cref="RemoveButtonUpListener"/>
		public static event Action OnRightMouseButtonUp
		{
			add => AddButtonUpListener(value, MouseButton.Right);
			remove => RemoveButtonUpListener(value, MouseButton.Right);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MiddleMouseButtonDown
		/// </summary>
		/// <seealso cref="AddButtonDownListener"/>
		/// <seealso cref="RemoveButtonDownListener"/>
		public static event Action OnMiddleMouseButtonDown
		{
			add => AddButtonDownListener(value, MouseButton.Middle);
			remove => RemoveButtonDownListener(value, MouseButton.Middle);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MiddleMouseButtonDown
		/// </summary>
		/// <seealso cref="AddButtonUpListener"/>
		/// <seealso cref="RemoveButtonUpListener"/>
		public static event Action OnMiddleMouseButtonUp
		{
			add => AddButtonUpListener(value, MouseButton.Middle);
			remove => RemoveButtonUpListener(value, MouseButton.Middle);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MouseBackButtonDown
		/// </summary>
		/// <seealso cref="AddButtonDownListener"/>
		/// <seealso cref="RemoveButtonDownListener"/>
		public static event Action OnMouseBackButtonDown
		{
			add => AddButtonDownListener(value, MouseButton.Back);
			remove => RemoveButtonDownListener(value, MouseButton.Back);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MouseBackButtonUp
		/// </summary>
		/// <seealso cref="AddButtonUpListener"/>
		/// <seealso cref="RemoveButtonUpListener"/>
		public static event Action OnMouseBackButtonUp
		{
			add => AddButtonUpListener(value, MouseButton.Back);
			remove => RemoveButtonUpListener(value, MouseButton.Back);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MouseForwardButtonDown
		/// </summary>
		/// <seealso cref="AddButtonDownListener"/>
		/// <seealso cref="RemoveButtonDownListener"/>
		public static event Action OnMouseForwardButtonDown
		{
			add => AddButtonDownListener(value, MouseButton.Forward);
			remove => RemoveButtonDownListener(value, MouseButton.Forward);
		}

		/// <summary>
		/// A simplified way to to add/remove listeners to MouseForwardButtonUp
		/// </summary>
		/// <seealso cref="AddButtonUpListener"/>
		/// <seealso cref="RemoveButtonUpListener"/>
		public static event Action OnMouseForwardButtonUp
		{
			add => AddButtonUpListener(value, MouseButton.Forward);
			remove => RemoveButtonUpListener(value, MouseButton.Forward);
		}

		/// <summary>
		/// Invoked when the mouse wheel scrolls
		/// <para>The parameter is a simplified way to get the scroll delta</para>
		/// <para>use <see cref="IsScrolling"/> to query whether the mouse is scrolling at any time</para>
		/// <para>WARNING: will be invoked every frame that the mouse scrolls</para>
		/// </summary>
		/// <seealso cref="MouseScrollDelta"/>
		/// <seealso cref="IsScrolling"/>
		/// <seealso cref="AddScrollListener"/>
		/// <seealso cref="RemoveScrollListener"/>
		public static event Action<Vector2> OnScroll
		{
			add => AddScrollListener(value);
			remove => RemoveScrollListener(value);
		}

		#endregion

		#region Private Events

		/// <seealso cref="OnScroll"/>
		private static event Action<Vector2> onMouseScroll = delegate { };

		private static event Action anyMouseButtonDown = delegate { };
		private static event Action anyMouseButtonUp = delegate { };

		#endregion

		#region Properties

		/// <summary>
		/// Is any mouse button currently down?
		/// </summary>
		public static bool IsAnyMouseButtonDown => mouseButtonHandlers.Any(handler => handler.ButtonPressed);

		/// <summary>
		///<para>The amount the scroll wheel was scrolled relative to the last frame</para>
		///<para>X value can be non-zero for horizontal scrolls (e.g. with touchpad)</para>
		/// </summary>
		public static Vector2 MouseScrollDelta => GetMouseScroll();

		/// <summary>
		/// Returns true if the <see cref="MouseScrollDelta"/> has a value higher than 0
		/// </summary>
		public static bool IsScrolling => MouseScrollDelta.sqrMagnitude > 0.0f;

		#endregion

		#region Private fields

		private static readonly MouseInputEventHandler[] mouseButtonHandlers = new MouseInputEventHandler[5];

		#endregion

		#region Button Listener Methods

		/// <summary>
		///<para>Add a listener to the AnyMouseButtonDown event</para>
		///<para>This will initialize the MouseUtil singleton if it has not been initialized yet</para>
		/// </summary>
		/// <param name="callback">the callback to add to the event</param>
		public static void AddAnyButtonDownListener(Action callback)
		{
			if (!IsInitialized)
			{
				ForceInitialize();
			}

			anyMouseButtonDown += callback;
		}

		/// <summary>
		///<para>Remove a listener from the AnyMouseButtonDown event</para>
		/// </summary>
		/// <param name="callback">the callback to remove from the event</param>
		public static void RemoveAnyButtonDownListener(Action callback)
		{
			anyMouseButtonDown -= callback;
		}

		/// <summary>
		///<para>Add a listener to the AnyMouseButtonUp event</para>
		///<para>This will initialize the MouseUtil singleton if it has not been initialized yet</para>
		/// </summary>
		/// <param name="callback">the callback to add to the event</param>
		public static void AddAnyButtonUpListener(Action callback)
		{
			if (!IsInitialized)
			{
				ForceInitialize();
			}

			anyMouseButtonUp += callback;
		}

		/// <summary>
		///<para>Remove a listener from the AnyMouseButtonUp event</para>
		/// </summary>
		/// <param name="callback">the callback to remove from the event</param>
		public static void RemoveAnyButtonUpListener(Action callback)
		{
			anyMouseButtonUp -= callback;
		}

		/// <summary>
		///<para>Add a listener to the respective MouseButtonDown event</para>
		/// <para>This will initialize the MouseUtil singleton if it has not been initialized yet</para>
		/// </summary>
		/// <param name="callback">the callback to add to the event</param>
		/// <param name="mouseButton">The mousebutton whose MouseButtonDown event to subscribe to</param>
		public static void AddButtonDownListener(Action callback, MouseButton mouseButton = MouseButton.Left)
		{
			if (!IsInitialized)
			{
				ForceInitialize();
			}

			mouseButtonHandlers[(int)mouseButton].OnButtonDown += callback;
		}

		/// <summary>
		///<para>Remove a listener from the respective MouseButtonDown event</para>
		/// </summary>
		/// <param name="callback">the callback to remove from the event</param>
		/// <param name="mouseButton">The mousebutton whose MouseButtonDown event to unsubscribe from</param>
		public static void RemoveButtonDownListener(Action callback, MouseButton mouseButton = MouseButton.Left)
		{
			MouseInputEventHandler handler = mouseButtonHandlers[(int)mouseButton];

			if (handler == null) // If it is null, it has never been intialised yet so we can skip it
			{
				return;
			}

			mouseButtonHandlers[(int)mouseButton].OnButtonDown -= callback;
		}

		/// <summary>
		///<para>Add a listener to the respective MouseButtonUp event</para>
		///<para>This will initialize the MouseUtil singleton if it has not been initialized yet</para>
		/// </summary>
		/// <param name="callback">the callback to add to the event</param>
		/// <param name="mouseButton">The mousebutton whose MouseButtonUp event to subscribe to</param>
		public static void AddButtonUpListener(Action callback, MouseButton mouseButton = MouseButton.Left)
		{
			if (!IsInitialized)
			{
				ForceInitialize();
			}

			mouseButtonHandlers[(int)mouseButton].OnButtonUp += callback;
		}

		/// <summary>
		///<para>Remove a listener from the respective MouseButtonUp event</para>
		/// </summary>
		/// <param name="callback">the callback to remove from the event</param>
		/// <param name="mouseButton">The mousebutton whose MouseButtonUp event to unsubscribe from</param>
		public static void RemoveButtonUpListener(Action callback, MouseButton mouseButton = MouseButton.Left)
		{
			MouseInputEventHandler handler = mouseButtonHandlers[(int)mouseButton];

			if (handler == null) // If it is null, it has never been intialised yet so we can skip it
			{
				return;
			}

			mouseButtonHandlers[(int)mouseButton].OnButtonUp -= callback;
		}

		/// <summary>
		///<para>Add a listener to the MouseScroll event</para>
		/// <para>This will initialize the MouseUtil singleton if it has not been initialized yet</para>
		/// </summary>
		/// <param name="callback">the callback to add to the event</param>
		/// <seealso cref="OnScroll"/>
		public static void AddScrollListener(Action<Vector2> callback)
		{
			if (!IsInitialized)
			{
				ForceInitialize();
			}

			onMouseScroll += callback;
		}

		/// <summary>
		///<para>Remove a listener from the MouseScroll event</para>
		/// </summary>
		/// <param name="callback">the callback to remove from the event</param>
		/// <seealso cref="OnScroll"/>
		public static void RemoveScrollListener(Action<Vector2> callback)
		{
			onMouseScroll -= callback;
		}

		#endregion

		#region Button Query Methods

		/// <summary>
		/// Checks whether the specified MouseButton is currently pressed
		/// </summary>
		/// <param name="mouseButton">The MouseButton to check (defaults to left mouse button)</param>
		/// <returns></returns>
		public static bool IsButtonPressed(MouseButton mouseButton = MouseButton.Left)
		{
			return mouseButtonHandlers[(int)mouseButton].ButtonPressed;
		}

		#endregion

		#region Private Methods

		private static void InvokeAnyButtonDown()
		{
			anyMouseButtonDown.Invoke();
		}

		private static void InvokeAnyButtonUp()
		{
			anyMouseButtonUp.Invoke();
		}

		#endregion

		#region UNITY_INPUT_SYSTEM

#if UNITY_INPUT_SYSTEM
		protected override void Awake()
		{
			base.Awake();

			if (!mouseButtonHandlers.Any(handler => ReferenceEquals(handler, null))) // Since it's static it could already be initialised
			{
				return;
			}

			for (int i = 0; i < mouseButtonHandlers.Length; i++)
			{
				MouseButton mouseButton = (MouseButton)i;
				mouseButtonHandlers[i] = new MouseInputEventHandler(GetButtonPressedThisFrame(mouseButton), GetButtonReleasedThisFrame(mouseButton));
			}

			if (!transform.parent)
			{
				DontDestroyOnLoad(true);
			}
		}

		private static Vector2 GetMouseScroll()
		{
			return Mouse.current.scroll.ReadValue();
		}

		private static Func<bool> GetButtonPressedThisFrame(MouseButton mouseButton)
		{
			return mouseButton switch
			{
				MouseButton.Left => () => Mouse.current.leftButton.wasPressedThisFrame,
				MouseButton.Right => () => Mouse.current.rightButton.wasPressedThisFrame,
				MouseButton.Middle => () => Mouse.current.middleButton.wasPressedThisFrame,
				MouseButton.Forward => () => Mouse.current.forwardButton.wasPressedThisFrame,
				MouseButton.Back => () => Mouse.current.backButton.wasPressedThisFrame,
				_ => throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, "Not a valid mouse button"),
			};
		}

		private static Func<bool> GetButtonReleasedThisFrame(MouseButton mouseButton)
		{
			return mouseButton switch
			{
				MouseButton.Left => () => Mouse.current.leftButton.wasReleasedThisFrame,
				MouseButton.Right => () => Mouse.current.rightButton.wasReleasedThisFrame,
				MouseButton.Middle => () => Mouse.current.middleButton.wasReleasedThisFrame,
				MouseButton.Forward => () => Mouse.current.forwardButton.wasReleasedThisFrame,
				MouseButton.Back => () => Mouse.current.backButton.wasReleasedThisFrame,
				_ => throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, "Not a valid mouse button"),
			};
		}

#endif

		#endregion

		#region Legacy input

#if !UNITY_INPUT_SYSTEM
		protected override void Awake()
		{
			base.Awake();

			if (!mouseButtonHandlers.Any(handler => ReferenceEquals(handler, null))) // Since it's static it could already be initialised
			{
				return;
			}

			/*
			 * [0] == Left
			 * [1] == Right
			 * [2] == Mouse Wheel button
			 * [3] == Mouse Back
			 * [4] == Mouse Forward
			 */
			for (int i = 0; i < mouseButtonHandlers.Length; i++)
			{
				mouseButtonHandlers[i] = new MouseInputEventHandler(GetMouseButtonDown(i), GetMouseButtonUp(i));
			}

			if (!transform.parent)
			{
				DontDestroyOnLoad(true);
			}
		}

		private static Vector2 GetMouseScroll()
		{
			return Input.mouseScrollDelta;
		}

		private static Func<bool> GetMouseButtonDown(int buttonNumber)
		{
			return () => Input.GetMouseButtonDown(buttonNumber);
		}

		private static Func<bool> GetMouseButtonUp(int buttonNumber)
		{
			return () => Input.GetMouseButtonUp(buttonNumber);
		}
#endif

		#endregion

		#region Unity Event Functions

		private void Update()
		{
			// Will be non-null if it has listeners, this allows us to prevent the Vector math for GetScrollDelta when we don't need it
			if (onMouseScroll != null && IsScrolling)
			{
				onMouseScroll.Invoke(GetMouseScroll());
			}

			foreach (MouseInputEventHandler buttonHandler in mouseButtonHandlers)
			{
				buttonHandler.Update();
			}
		}

		#endregion
	}
}