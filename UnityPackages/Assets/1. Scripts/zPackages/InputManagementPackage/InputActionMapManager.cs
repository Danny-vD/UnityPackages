using System.Collections.Generic;
using InputManagementPackage.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using VDFramework.Extensions;
using VDFramework.Singleton;

namespace InputManagementPackage
{
	public class InputActionMapManager : Singleton<InputActionMapManager>
	{
		[SerializeField]
		private InputActionAsset inputActionAsset;

		private readonly Dictionary<ControlType, InputActionMap> actionMapsByType = new Dictionary<ControlType, InputActionMap>();

		public ControlType CurrentControlType { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			SetActionsMapsPerControlType();

			CurrentControlType = default;
			SetControls(default);
		}

		/// <summary>
		/// Disables the current action map and enables the action map mapped to the given <see cref="ControlType"/><br/>
		/// This function does not do anything if the given <see cref="ControlType"/> is already the currently used action map
		/// </summary>
		/// <param name="controlType"></param>
		public void ChangeControls(ControlType controlType)
		{
			if (controlType == CurrentControlType)
			{
				return;
			}

			SetControls(controlType);
		}

		public InputActionMap GetActionMap(ControlType controlType)
		{
			return actionMapsByType[controlType];
		}

		private void SetControls(ControlType controlType)
		{
			actionMapsByType[CurrentControlType].Disable();
			actionMapsByType[controlType].Enable();
			CurrentControlType = controlType;
		}

		private void SetActionsMapsPerControlType()
		{
			IEnumerator<ControlType> controlTypes = default(ControlType).GetValues().GetEnumerator();

			foreach (InputActionMap inputActionMap in inputActionAsset.actionMaps)
			{
				if (!controlTypes.MoveNext())
				{
					break;
				}

				actionMapsByType[controlTypes.Current] = inputActionMap;
			}

			controlTypes.Dispose();
		}
	}
}