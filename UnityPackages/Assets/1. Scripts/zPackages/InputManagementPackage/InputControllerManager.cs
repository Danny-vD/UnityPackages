using System.Collections.Generic;
using System.Linq;
using InputManagementPackage.Enum;
using UnityEngine;
using UnityEngine.InputSystem;
using VDFramework.Extensions;
using VDFramework.Singleton;

namespace InputManagementPackage
{
	public class InputControllerManager : Singleton<InputControllerManager>
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

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (inputActionAsset != null)
			{
				CreateEnums();
			}
		}

		private void CreateEnums()
		{
			if (default(ControlType).GetValues().Count() == inputActionAsset.actionMaps.Count)
			{
				return;
			}

			int enumValue = 0;
			List<string> enumNames = new List<string>();

			foreach (InputActionMap inputActionMap in inputActionAsset.actionMaps)
			{
				ControlType controlType = (ControlType)enumValue;

				actionMapsByType[controlType] = inputActionMap;

				enumNames.Add(inputActionMap.name);

				++enumValue;
			}

			EnumWriter.WriteToEnum<ControlType>(enumNames, null);
		}
#endif
	}
}