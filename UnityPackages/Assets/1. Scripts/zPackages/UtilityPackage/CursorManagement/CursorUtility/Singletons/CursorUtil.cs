using UnityEngine;
using UnityEngine.EventSystems;
using VDFramework.Singleton;

#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

#else
using System.Reflection;
#endif

namespace UtilityPackage.CursorManagement.CursorUtility.Singletons
{
	public class CursorUtil : Singleton<CursorUtil>
	{
#if UNITY_INPUT_SYSTEM
		private InputSystemUIInputModule inputModule;

#else
		private static EventSystem EventSystem => EventSystem.current;

		/// <inheritdoc cref="UnityEngine.EventSystems.PointerInputModule.GetPointerData"/>
		/// <function>protected bool GetPointerData(int id, out PointerEventData data, bool create)</function>
		private MethodInfo getPointerData;

		private readonly object[] parameters = new object[] { PointerInputModule.kMouseLeftId, null, false };

#endif

		/// <summary>
		/// The current cursor position in ScreenSpace
		/// </summary>
		public static Vector3 CursorPosition => GetCursorPosition();

		/// <summary>
		/// The current cursor position in ScreenSpace with 0 as Z value
		/// </summary>
		public static Vector2 CursorPosition2D => GetCursorPosition2D();

#if UNITY_INPUT_SYSTEM
		private void OnEnable()
		{
			if (inputModule == null)
			{
				inputModule = FindObjectOfType<InputSystemUIInputModule>(false);

				if (inputModule == null)
				{
					Debug.LogErrorFormat("No {0} found in the scene!", nameof(InputSystemUIInputModule));
				}
			}
		}

		public bool TryGetHoveredGameObject(int pointerID, out GameObject hoveredGameObject)
		{
			if (inputModule.IsPointerOverGameObject(pointerID))
			{
				RaycastResult raycastResult = inputModule.GetLastRaycastResult(pointerID);

				if (raycastResult.isValid)
				{
					hoveredGameObject = raycastResult.gameObject;
					return true;
				}
			}

			hoveredGameObject = null;
			return false;
		}

		public bool TryGetHoveredGameObject(out GameObject hoveredGameObject)
		{
			return TryGetHoveredGameObject(Mouse.current.deviceId, out hoveredGameObject);
		}
#else
		protected override void Awake()
		{
			base.Awake();

			getPointerData = typeof(PointerInputModule).GetMethod("GetPointerData", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public bool TryGetHoveredGameObject(int pointerID, out GameObject hoveredGameObject)
		{
			PointerInputModule pointerInputModule = EventSystem.currentInputModule as PointerInputModule;

			if (pointerInputModule != null && EventSystem.IsPointerOverGameObject(pointerID))
			{
				parameters[0] = pointerID;

				getPointerData.Invoke(pointerInputModule, parameters); // The returned value is not relevant (it's always false)

				PointerEventData pointerEventData = (PointerEventData)parameters[1];

				if (pointerEventData != null)
				{
					hoveredGameObject = pointerEventData.pointerCurrentRaycast.gameObject;
					return true;
				}
			}

			hoveredGameObject = null;
			return false;
		}

		public bool TryGetHoveredGameObject(out GameObject hoveredGameObject)
		{
			return TryGetHoveredGameObject(PointerInputModule.kMouseLeftId, out hoveredGameObject);
		}
#endif

		#region CursorWorldPosition Methods

		/// <summary>
		/// Get the CursorPosition in world space
		/// </summary>
		/// <param name="camera">The camera from which to calculate cursor world position</param>
		/// <param name="eye">By default, <see cref="Camera.MonoOrStereoscopicEye.Mono"/>. Can be set to <see cref="Camera.MonoOrStereoscopicEye.Left"/> or <see cref="Camera.MonoOrStereoscopicEye.Right"/> for use in stereoscopic rendering (e.g., for VR).</param>
		/// <returns>The cursor position in 3D world space</returns>
		public static Vector3 GetCursorWorldPosition(Camera camera, Camera.MonoOrStereoscopicEye eye = Camera.MonoOrStereoscopicEye.Mono)
		{
			Vector3 cursorPosition = GetCursorPosition();

#if UNITY_INPUT_SYSTEM

			// Cursor position is a vector 2 in the new input system
			cursorPosition.z = camera.nearClipPlane;
#endif

			return camera.ScreenToWorldPoint(cursorPosition, eye);
		}

		/// <summary>
		/// Returns a ray going from camera through the cursor position.
		/// </summary>
		/// <param name="camera">The camera from which the ray starts</param>
		/// <param name="eye">By default, <see cref="Camera.MonoOrStereoscopicEye.Mono"/>. Can be set to <see cref="Camera.MonoOrStereoscopicEye.Left"/> or <see cref="Camera.MonoOrStereoscopicEye.Right"/> for use in stereoscopic rendering (e.g., for VR).</param>
		/// <returns>A ray from the camera to the cursor position</returns>
		public static Ray GetCursorToWorldRay(Camera camera, Camera.MonoOrStereoscopicEye eye = Camera.MonoOrStereoscopicEye.Mono)
		{
			return camera.ScreenPointToRay(GetCursorPosition(), eye);
		}

		#endregion

#if UNITY_INPUT_SYSTEM
		private static Vector3 GetCursorPosition()
		{
			return GetCursorPosition2D();
		}

		private static Vector2 GetCursorPosition2D()
		{
			return Mouse.current.position.ReadValue();
		}

#else
		private static Vector3 GetCursorPosition()
		{
			return Input.mousePosition;
		}

		private static Vector2 GetCursorPosition2D()
		{
			return Input.mousePosition;
		}
#endif
	}
}