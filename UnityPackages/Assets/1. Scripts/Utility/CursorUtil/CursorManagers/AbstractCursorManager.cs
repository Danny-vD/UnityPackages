using UnityEngine;
using VDFramework;
using Utility.SerializableDictionary;
using MouseButton = Utility.CursorUtil.MouseUtil.MouseButton;

namespace Utility.CursorUtil.CursorManagers
{
	[DisallowMultipleComponent]
	public abstract class AbstractCursorManager<T> : BetterMonoBehaviour where T : struct
	{
		[SerializeField]
		protected T idleDatum;
		
		[SerializeField, Tooltip("Order matters, higher placed will be shown over lower")]
		private SerializableDictionary<MouseButton, T> MouseClickData;
		
		public bool IsIdle { get; private set; }

		public MouseButton CurrentState { get; private set; }
		
		private void Update()
		{
			foreach ((MouseButton button, T datum) in MouseClickData)
			{
				if (MouseUtil.IsButtonPressed(button))
				{
					if (!IsIdle && CurrentState == button) // If we're currently idle, always go through
					{
						// Prevent constantly setting the current state to itself
						return;
					}

					SetState(button, datum);
					return;
				}
			}

			if (!IsIdle)
			{
				SetState(default, idleDatum, true);
			}
		}

		private void SetState(MouseButton button, T value, bool idle = false)
		{
			CurrentState = button;
			IsIdle       = idle;
			
			OnStateChanged(CurrentState, value, IsIdle);
		}

		protected abstract void OnStateChanged(MouseButton newState, T datum, bool idle = false);
	}
}