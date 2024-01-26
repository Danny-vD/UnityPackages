using System.Globalization;
using UnityEditor;
using UnityEngine;
using UtilityPackage.Attributes;

namespace PropertyDrawers.Attributes.EditorPackage
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyAttributeEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.LabelField(position, $"{property.displayName}\t{GetPropertyValue(property)}");
		}

		private static string GetPropertyValue(SerializedProperty property)
		{
			string defaultText = $"{property.type}";

			switch (property.propertyType)
			{
				case SerializedPropertyType.Generic:
					return defaultText;

				case SerializedPropertyType.Integer:
					return property.intValue.ToString();

				case SerializedPropertyType.Boolean:
					return property.boolValue.ToString();

				case SerializedPropertyType.Float:
					return property.floatValue.ToString(CultureInfo.InvariantCulture);

				case SerializedPropertyType.String:
					string stringValue = property.stringValue;

					return string.IsNullOrEmpty(stringValue) ? nameof(string.Empty) : property.stringValue;

				case SerializedPropertyType.Color:
					return property.colorValue.ToString();

				case SerializedPropertyType.ObjectReference:
					Object @object = property.objectReferenceValue;

					return @object == null ? "Null" : @object.name;

				case SerializedPropertyType.LayerMask:
					return defaultText;

				case SerializedPropertyType.Enum:
					string[] enumNames = property.enumNames;

					if (property.enumValueIndex < 0 || property.enumValueIndex > enumNames.Length)
					{
						return property.intValue.ToString();
					}

					return property.enumNames[property.enumValueIndex];

				case SerializedPropertyType.Vector2:
					return property.vector2Value.ToString();

				case SerializedPropertyType.Vector3:
					return property.vector3Value.ToString();

				case SerializedPropertyType.Vector4:
					return property.vector4Value.ToString();

				case SerializedPropertyType.Rect:
					return property.rectValue.ToString();

				case SerializedPropertyType.ArraySize:
					return property.intValue.ToString();

				case SerializedPropertyType.Character:
					char value = (char)property.intValue;

					return value == ' ' ? "Space" : value.ToString();

				case SerializedPropertyType.AnimationCurve:
					return property.animationCurveValue.ToString();

				case SerializedPropertyType.Bounds:
					return property.boundsValue.ToString();

				case SerializedPropertyType.Gradient:
					return defaultText;

				case SerializedPropertyType.Quaternion:
					return property.quaternionValue.ToString();

				case SerializedPropertyType.ExposedReference:
					return property.exposedReferenceValue.name;

				case SerializedPropertyType.FixedBufferSize:
					return property.fixedBufferSize.ToString();

				case SerializedPropertyType.Vector2Int:
					return property.vector2IntValue.ToString();

				case SerializedPropertyType.Vector3Int:
					return property.vector3IntValue.ToString();

				case SerializedPropertyType.RectInt:
					return property.rectIntValue.ToString();

				case SerializedPropertyType.BoundsInt:
					return property.boundsIntValue.ToString();

				case SerializedPropertyType.ManagedReference:
					return property.managedReferenceFullTypename;

				default:
					return defaultText;
			}
		}
	}
}