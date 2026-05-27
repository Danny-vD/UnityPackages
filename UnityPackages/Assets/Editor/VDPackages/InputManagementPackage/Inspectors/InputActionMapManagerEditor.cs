using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem;
using VDFramework.Extensions;
using VDPackages;
using VDPackages.InputManagementPackage;
using VDPackages.InputManagementPackage.Enums;
using VDPackagesEditor.UtilityPackage;

namespace VDPackagesEditor.InputManagementPackage.Inspectors
{
	[CustomEditor(typeof(InputActionMapManager))]
	public class InputActionMapManagerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
			{
				base.OnInspectorGUI();

				if (changeCheckScope.changed)
				{
					SerializedProperty serializedProperty = serializedObject.FindProperty("inputActionAsset");
					WriteActionMapsToEnum((InputActionAsset)serializedProperty.objectReferenceValue);
				}
			}
		}

		private void WriteActionMapsToEnum(InputActionAsset inputActionAsset)
		{
			if (inputActionAsset != null)
			{
				string[] actionMapNames = inputActionAsset.actionMaps.Select(inputActionMap => inputActionMap.name).ToArray();
				
				if (ControlTypesEnumNeedsUpdating(actionMapNames))
				{
					EnumWriter.WriteEnumValuesAutomaticPath<ControlType>(VDPackagesConstants.PACKAGES_PATH, actionMapNames, null);
				}
			}
		}

		/// <summary>
		/// Checks if the enum has an equal amount of values as the actionMaps count and if they are named the same
		/// </summary>
		private static bool ControlTypesEnumNeedsUpdating(string[] actionMapNames)
		{
			string[] enumValues = default(ControlType).GetValues().Select(value => value.ToString()).ToArray();

			if (actionMapNames.Length != enumValues.Length)
			{
				return true;
			}
			
			for (int i = 0; i < actionMapNames.Length; i++)
			{
				string actionMapName = actionMapNames[i];
				string enumValue = enumValues[i];

				if (!actionMapName.Equals(enumValue))
				{
					return true;
				}
			}

			return false;
		}
	}
}