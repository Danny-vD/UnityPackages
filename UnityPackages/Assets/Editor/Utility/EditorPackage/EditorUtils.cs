using System;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;

namespace Utility.EditorPackage
{
	public static class EditorUtils
	{
		private static readonly Color grey = new Color(0.4f, 0.4f, 0.4f);

		/// <summary>
		/// Creates a foldout label
		/// </summary>
		public static bool IsFoldOut(ref bool foldout, string label = "")
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = GetLabelWidth(label);

			foldout = EditorGUILayout.Foldout(foldout, label);

			EditorGUIUtility.labelWidth = oldLabelWidth;
			return foldout;
		}

		/// <summary>
		/// Creates a foldout label with a texture
		/// </summary>
		public static bool IsFoldOut(ref bool foldout, Texture icon, string label = "")
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth =
				GetLabelWidth(label) + 200.0f; // Enough space to fit the text and some extra for the texture

			foldout = EditorGUILayout.Foldout(foldout, new GUIContent(label, icon));

			EditorGUIUtility.labelWidth = oldLabelWidth;
			return foldout;
		}

		/// <summary>
		/// A simplified way to get a texture from "Assets/Editor Default Resources" or a built-in texture
		/// </summary>
		public static Texture GetTexture(string path)
		{
			return EditorGUIUtility.Load(path) as Texture;
		}

		#region Vectors

		/// <summary>
		/// Draws a vector2 property with custom labels and variable width
		/// </summary>
		public static Vector2 DrawVector2(float x, string xLabel, float y, string yLabel)
		{
			Vector2 vector2 = Vector2.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector2.x = FlexibleFloatField(ref x, xLabel);
				vector2.y = FlexibleFloatField(ref y, yLabel);
			}
			EditorGUILayout.EndHorizontal();

			return vector2;
		}

		/// <summary>
		/// Draws a vector3 property with custom labels and variable width
		/// </summary>
		public static Vector3 DrawVector3(float x, string xLabel, float y, string yLabel, float z, string zLabel)
		{
			Vector3 vector3 = Vector3.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector3.x = FlexibleFloatField(ref x, xLabel);
				vector3.y = FlexibleFloatField(ref y, yLabel);
				vector3.z = FlexibleFloatField(ref z, zLabel);
			}
			EditorGUILayout.EndHorizontal();

			return vector3;
		}

		/// <summary>
		/// Draws a vector4 property with custom labels and variable width
		/// </summary>
		public static Vector4 DrawVector4(
			float  x,
			string xLabel,
			float  y,
			string yLabel,
			float  z,
			string zLabel,
			float  w,
			string wLabel
		)
		{
			Vector4 vector4 = Vector4.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector4.x = FlexibleFloatField(ref x, xLabel);
				vector4.y = FlexibleFloatField(ref y, yLabel);
				vector4.z = FlexibleFloatField(ref z, zLabel);
				vector4.z = FlexibleFloatField(ref w, wLabel);
			}
			EditorGUILayout.EndHorizontal();

			return vector4;
		}

		/// <summary>
		/// Draws a vector2Int property with custom labels and variable width
		/// </summary>
		public static Vector2Int DrawVector2Int(int x, string xLabel, int y, string yLabel)
		{
			Vector2Int vector2 = Vector2Int.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector2.x = FlexibleIntField(ref x, xLabel);
				vector2.y = FlexibleIntField(ref y, yLabel);
			}
			EditorGUILayout.EndHorizontal();

			return vector2;
		}

		/// <summary>
		/// Draws a vector3Int property with custom labels and variable width
		/// </summary>
		public static Vector3Int DrawVector3Int(int x, string xLabel, int y, string yLabel, int z, string zLabel)
		{
			Vector3Int vector3 = Vector3Int.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector3.x = FlexibleIntField(ref x, xLabel);
				vector3.y = FlexibleIntField(ref y, yLabel);
				vector3.z = FlexibleIntField(ref z, zLabel);
			}
			EditorGUILayout.EndHorizontal();


			return vector3;
		}

		/// <summary>
		/// Draws a vector4Int property with custom labels and variable width
		/// </summary>
		public static Vector4 DrawVector4Int(
			int    x,
			string xLabel,
			int    y,
			string yLabel,
			int    z,
			string zLabel,
			int    w,
			string wLabel
		)
		{
			Vector4 vector4 = Vector4.zero;

			EditorGUILayout.BeginHorizontal();
			{
				vector4.x = FlexibleIntField(ref x, xLabel);
				vector4.y = FlexibleIntField(ref y, yLabel);
				vector4.z = FlexibleIntField(ref z, zLabel);
				vector4.z = FlexibleIntField(ref w, wLabel);
			}
			EditorGUILayout.EndHorizontal();

			return vector4;
		}

		#endregion

		#region KeyValueArray

		// @formatter:off — prevent R# from formatting this block
		/// <summary>
		/// Calls elementAction(index, @struct) for every element in the array
		/// </summary>
		public static void DrawKeyValueArray(SerializedProperty array, Action<int, SerializedProperty> elementAction)
		{
			++EditorGUI.indentLevel;

			for (int i = 0; i < array.arraySize; i++)
			{
				SerializedProperty @struct = array.GetArrayElementAtIndex(i);

				elementAction(i, @struct);
			}

			--EditorGUI.indentLevel;
		}

		/// <summary>
		/// Calls elementAction(index, key, value) for every element in the array
		/// </summary>
		public static void DrawKeyValueArray(SerializedProperty array, string keyName, string valueName,
			Action<int, SerializedProperty, SerializedProperty> elementAction
		)
		{
			++EditorGUI.indentLevel;

			for (int i = 0; i < array.arraySize; i++)
			{
				SerializedProperty @struct = array.GetArrayElementAtIndex(i);
				SerializedProperty key = @struct.FindPropertyRelative(keyName);
				SerializedProperty value = @struct.FindPropertyRelative(valueName);

				elementAction(i, key, value);
			}

			--EditorGUI.indentLevel;
		}

		/// <summary>
		/// Will create a foldout label with a texture for every value in the enum and call elementAction(index, @struct) for every element
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(
			SerializedProperty array, string keyName, bool[] foldouts,
			Texture[] keyTextures, Action<int, SerializedProperty> elementAction
		)
			where TEnum : struct, Enum
		{
			DrawKeyValueArray(array, DrawFoldout);

			void DrawFoldout(int i, SerializedProperty @struct)
			{
				string enumString = ConvertIntToEnum<TEnum>(@struct.FindPropertyRelative(keyName).enumValueIndex)
					.ToString()
					.ReplaceUnderscoreWithSpace();

				if (IsFoldOut(ref foldouts[i], keyTextures[i % keyTextures.Length], enumString))
				{
					++EditorGUI.indentLevel;
					elementAction(i, @struct);
					--EditorGUI.indentLevel;
				}
			}
		}

		/// <summary>
		/// Will create a foldout label with a texture for every value in the enum and call elementAction(index, key, value) for every element
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, string keyName, string valueName, bool[] foldouts,
			Texture[] keyTextures, Action<int, SerializedProperty, SerializedProperty> elementAction
		)
			where TEnum : struct, Enum
		{
			DrawKeyValueArray(array, keyName, valueName, DrawFoldout);

			void DrawFoldout(int i, SerializedProperty key, SerializedProperty value)
			{
				string enumString = ConvertIntToEnum<TEnum>(key.enumValueIndex).ToString().ReplaceUnderscoreWithSpace();

				if (IsFoldOut(ref foldouts[i], keyTextures[i % keyTextures.Length], enumString))
				{
					++EditorGUI.indentLevel;
					elementAction(i, key, value);
					--EditorGUI.indentLevel;
				}
			}
		}

		/// <summary>
		/// Will create a foldout label for every value in the enum and call elementAction(index, @struct) for every element
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, string keyName, bool[] foldouts,
			Action<int, SerializedProperty> elementAction
		)
			where TEnum : struct, Enum
		{
			DrawKeyValueArray(array, DrawFoldout);

			void DrawFoldout(int i, SerializedProperty @struct)
			{
				string enumString = ConvertIntToEnum<TEnum>(@struct.FindPropertyRelative(keyName).enumValueIndex)
					.ToString()
					.ReplaceUnderscoreWithSpace();

				if (IsFoldOut(ref foldouts[i], enumString))
				{
					++EditorGUI.indentLevel;
					elementAction(i, @struct);
					--EditorGUI.indentLevel;
				}
			}
		}
		
		/// <summary>
		/// Will create a foldout label for every value in the enum and call elementAction(index, key, value) for every element
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, string keyName, string valueName,bool[] foldouts,
			Action<int, SerializedProperty, SerializedProperty> elementAction)
			where TEnum : struct, Enum
		{
			DrawKeyValueArray(array, keyName, valueName, DrawFoldout);

			void DrawFoldout(int i, SerializedProperty key, SerializedProperty value)
			{
				string enumString = ConvertIntToEnum<TEnum>(key.enumValueIndex).ToString().ReplaceUnderscoreWithSpace();

				if (IsFoldOut(ref foldouts[i], enumString))
				{
					++EditorGUI.indentLevel;
					elementAction(i, key, value);
					--EditorGUI.indentLevel;
				}
			}
		}

		/// <summary>
		/// Will create a foldout label with a texture for every value in the enum and draw every value with a specified label
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, string keyName, string valueName, bool[] foldouts,
			Texture[] keyTextures, GUIContent valueLabel)
			where TEnum : struct, Enum
		{
			DrawFoldoutKeyValueArray<TEnum>(array, keyName, valueName, foldouts, keyTextures, DrawValue);

			void DrawValue(int i, SerializedProperty key, SerializedProperty value)
			{
				EditorGUILayout.PropertyField(value, valueLabel);
			}
		}

		/// <summary>
		/// Will create a foldout label for every value in the enum and draw every value with a specified label
		/// </summary>
		public static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, string keyName, string valueName, bool[] foldouts,
			GUIContent valueLabel)
			where TEnum : struct, Enum
		{
			DrawFoldoutKeyValueArray<TEnum>(array, keyName, valueName, foldouts, DrawValue);

			void DrawValue(int i, SerializedProperty key, SerializedProperty value)
			{
				EditorGUILayout.PropertyField(value, valueLabel);
			}
		}
		// @formatter:on — enable formatter after this line

		#endregion

		/// <summary>
		/// Will draw an adjustable-size list with custom labels and a texture and calls elementAction(index, array[i]) for every element
		/// </summary>
		public static void DrawArray(
			SerializedProperty              array,
			string                          propertyName,
			Texture                         propertyIcon,
			string                          sizeLabel,
			Action<int, SerializedProperty> elementAction,
			ref bool                        isFoldOut
		)
		{
			if (IsFoldOut(ref isFoldOut, propertyIcon, propertyName))
			{
				++EditorGUI.indentLevel;
				int size = array.arraySize;

				array.arraySize = FlexibleIntField(ref size, sizeLabel);

				for (int i = 0; i < size; i++)
				{
					elementAction(i, array.GetArrayElementAtIndex(i));
				}

				--EditorGUI.indentLevel;
			}
		}

		/// <summary>
		/// Will draw an adjustable-size list with custom labels and calls elementAction(index, array[i]) for every element
		/// </summary>
		public static void DrawArray(
			SerializedProperty              array,
			string                          propertyName,
			string                          sizeLabel,
			Action<int, SerializedProperty> elementAction,
			ref bool                        isFoldOut
		)
		{
			if (IsFoldOut(ref isFoldOut, propertyName))
			{
				++EditorGUI.indentLevel;
				int size = array.arraySize;

				array.arraySize = FlexibleIntField(ref size, sizeLabel);

				for (int i = 0; i < array.arraySize; i++)
				{
					elementAction(i, array.GetArrayElementAtIndex(i));
				}

				--EditorGUI.indentLevel;
			}
		}

		/// <summary>
		/// Converts an index to a specified enum value (useful for SerializedProperty.enumValueIndex)
		/// </summary>
		public static TEnum ConvertIntToEnum<TEnum>(int integer)
			where TEnum : struct, Enum
		{
			return default(TEnum).GetValues().ElementAt(integer);
		}

		/// <summary>
		/// A simplified way to create a dropdown menu of any enum type
		/// </summary>
		public static int EnumPopup<TEnum>(ref TEnum enumValue, string label)
			where TEnum : struct, Enum
		{
			Enum.TryParse(EditorGUILayout.EnumPopup(label, enumValue).ToString(), out enumValue);
			return enumValue.ConvertTo<int>();
		}

		/// <summary>
		/// A simplified way to create a dropdown menu of any enum type
		/// </summary>
		public static int EnumPopup<TEnum>(ref TEnum enumValue, GUIContent guiContent)
			where TEnum : struct, Enum
		{
			Enum.TryParse(EditorGUILayout.EnumPopup(guiContent, enumValue).ToString(), out enumValue);
			return enumValue.ConvertTo<int>();
		}

		/// <summary>
		/// Draws a 1 pixel thick, grey line
		/// </summary>
		public static void DrawSeperatorLine()
		{
			DrawUILine(grey, 1);
		}

		/// <summary>
		/// Draws a line in the editor
		/// </summary>
		public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			rect.height =  thickness;
			rect.y      += padding / 2.0f;
			rect.x      -= 2;
			rect.width  += 6;
			EditorGUI.DrawRect(rect, color);
		}

		/// <summary>
		/// Draws a float field with variable width and a label
		/// </summary>
		public static float FlexibleFloatField(ref float value, string label)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = GetLabelWidth(label);
			value                       = EditorGUILayout.FloatField(label, value, GUILayout.Width(GetNumberWidth(value)));

			EditorGUIUtility.labelWidth = oldLabelWidth;
			return value;
		}

		/// <summary>
		/// Draws an int field with variable width and a label
		/// </summary>
		public static int FlexibleIntField(ref int value, string label)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = GetLabelWidth(label);
			value                       = EditorGUILayout.IntField(label, value, GUILayout.Width(GetNumberWidth(value)));

			EditorGUIUtility.labelWidth = oldLabelWidth;
			return value;
		}

		/// <summary>
		/// Draws a selectable label that takes up only the minimum space
		/// </summary>
		public static void FlexibleSelectableLabel(string label)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = GetLabelWidth(label);

			EditorGUILayout.SelectableLabel(label);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}

		/// <summary>
		/// Draws a LabelField that takes up only the minimum space
		/// </summary>
		public static void FlexibleLabel(string label)
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = GetLabelWidth(label);

			EditorGUILayout.LabelField(label);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}

		/// <summary>
		/// Returns a string that specifies the type of the SerializedProperty
		/// </summary>
		/// <seealso cref="SerializedProperty.type"/>
		public static string GetTypeString(SerializedProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException(nameof(property), "SerializedProperty is null, check if the type has serializable fields");
			}

			string type = property.type;

			// Pattern: PPtr<$TYPE>
			if (type.Contains("$"))
			{
				int startIndex = type.IndexOf('$') + 1;
				type = type[startIndex..^1];
			}

			return type;
		}

		/// <summary>
		/// Retrieve the value of the serializedProperty as a string wile taking the propertyType into acccount
		/// </summary>
		/// <param name="serializedProperty">The property whose value to return</param>
		/// <param name="index">[OPTIONAL] The index of the serializedProperty if it's part of a collection</param>
		/// <param name="defaultText">[OPTIONAL] A string that will be returned if no other string is appropriate ('{index}' will be replaced with the value of the index parameter)</param>
		/// <param name="propertyType">[OPTIONAL] The Type of the serializedProperty</param>
		/// <returns>A string that represents the current value of the serializedProperty</returns>
		/// <seealso cref="SerializedPropertyType"/>
		public static string GetValueString(SerializedProperty serializedProperty, int index = 0, string defaultText = "Element {index}", Type propertyType = null)
		{
			if (defaultText.Contains("{index}"))
			{
				defaultText = defaultText.Replace("{index}", index.ToString());
			}

			switch (serializedProperty.propertyType)
			{
				case SerializedPropertyType.Generic:
					return defaultText;

				case SerializedPropertyType.Integer:
					return serializedProperty.intValue.ToString();

				case SerializedPropertyType.Boolean:
					return serializedProperty.boolValue.ToString();

				case SerializedPropertyType.Float:
					return serializedProperty.floatValue.ToString(CultureInfo.InvariantCulture);

				case SerializedPropertyType.String:
					string stringValue = serializedProperty.stringValue;

					return string.IsNullOrEmpty(stringValue) ? nameof(string.Empty) : serializedProperty.stringValue;

				case SerializedPropertyType.Color:
					return serializedProperty.colorValue.ToString();

				case SerializedPropertyType.ObjectReference:
					UnityEngine.Object obj = serializedProperty.objectReferenceValue;

					if (ReferenceEquals(obj, null))
					{
						return "Null";
					}

					// The op_Equality performs a lifetime check, we use this to check if an object was destroyed
					return obj == null ? "Destroyed Object" : obj.name;

				case SerializedPropertyType.LayerMask:
					return defaultText;

				case SerializedPropertyType.Enum:
					string[] enumNames = propertyType != null ? Enum.GetNames(propertyType) : serializedProperty.enumNames;

					if (serializedProperty.enumValueIndex < 0 || serializedProperty.enumValueIndex > enumNames.Length)
					{
						// int value tends to be more reliable than enumValueIndex for nested types
						if (serializedProperty.intValue > 0 && serializedProperty.intValue < enumNames.Length)
						{
							return enumNames[serializedProperty.intValue];
						}
						
						return serializedProperty.intValue.ToString();
					}
					
					return enumNames[serializedProperty.enumValueIndex];

				case SerializedPropertyType.Vector2:
					return serializedProperty.vector2Value.ToString();

				case SerializedPropertyType.Vector3:
					return serializedProperty.vector3Value.ToString();

				case SerializedPropertyType.Vector4:
					return serializedProperty.vector4Value.ToString();

				case SerializedPropertyType.Rect:
					return serializedProperty.rectValue.ToString();

				case SerializedPropertyType.ArraySize:
					return serializedProperty.intValue.ToString();

				case SerializedPropertyType.Character:
					char value = (char)serializedProperty.intValue;

					return value == ' ' ? "Space" : value.ToString();

				case SerializedPropertyType.AnimationCurve:
					return serializedProperty.animationCurveValue.ToString();

				case SerializedPropertyType.Bounds:
					return serializedProperty.boundsValue.ToString();

				case SerializedPropertyType.Gradient:
					return defaultText;

				case SerializedPropertyType.Quaternion:
					return serializedProperty.quaternionValue.ToString();

				case SerializedPropertyType.ExposedReference:
					return serializedProperty.exposedReferenceValue.name;

				case SerializedPropertyType.FixedBufferSize:
					return serializedProperty.fixedBufferSize.ToString();

				case SerializedPropertyType.Vector2Int:
					return serializedProperty.vector2IntValue.ToString();

				case SerializedPropertyType.Vector3Int:
					return serializedProperty.vector3IntValue.ToString();

				case SerializedPropertyType.RectInt:
					return serializedProperty.rectIntValue.ToString();

				case SerializedPropertyType.BoundsInt:
					return serializedProperty.boundsIntValue.ToString();

				case SerializedPropertyType.ManagedReference:
					return serializedProperty.managedReferenceFullTypename;

				default:
					return defaultText;
			}
		}

		/// <summary>
		/// 15 pixels per indent + 11 pixels per letter
		/// </summary>
		private static float GetLabelWidth(string label)
		{
			return 15 * EditorGUI.indentLevel + 11 * label.Length;
		}

		/// <summary>
		/// labelWidth + 15 pixels per digit
		/// </summary>
		private static float GetNumberWidth<TNumber>(TNumber number)
			where TNumber : struct
		{
			return EditorGUIUtility.labelWidth + 15 * number.ToString().Length;
		}
	}
}