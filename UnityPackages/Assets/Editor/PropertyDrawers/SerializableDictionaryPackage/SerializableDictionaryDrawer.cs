using System;
using System.Collections.Generic;
using System.Linq;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Utility;
using Utility.UtilityPackage;

namespace PropertyDrawers.SerializableDictionaryPackage
{
	[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
	public class SerializableDictionaryDrawer : PropertyDrawer
	{
		// Constants, for consistent layout
		private const float baseSize = 15f;
		
		private const float warningHeight = 45.0f;
		private const float spacingWarningToDictionary = 10.0f;

		private const float paddingAtBeginOfElement = 2.0f;
		private const float spacingBetweenPairValues = 5.0f;
		private const float paddingAtEndOfElement = 8f;
		
		private const float paddingAtEndOfProperty = 0.0f;

		private float propertySize;

		private SerializedProperty current;
		private SerializedProperty serializedDictionary;
		private GUIContent currentLabel;

		private ReorderableList reorderableList;
		private float headerHeight = 20f;
		private float elementHeight = 21f;

		private bool isFoldOut;
		private bool isValid;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			current      = property;
			currentLabel = label;

			EditorGUI.BeginProperty(position, label, property);
			propertySize = baseSize;

			serializedDictionary = property.FindPropertyRelative("serializedDictionary");
			isValid              = IsValidDictionary(serializedDictionary, out int actualCount);

			if (ReferenceEquals(reorderableList, null)) // Prevent creating a new list every update
			{
				reorderableList = new ReorderableList(current.serializedObject, serializedDictionary, isFoldOut, true, isFoldOut, isFoldOut);
				headerHeight    = reorderableList.headerHeight;
				elementHeight   = reorderableList.elementHeight;

				// Setup the reorderable list
				reorderableList.drawHeaderCallback      += DrawHeader;
				reorderableList.drawElementCallback     += DrawElement;
				reorderableList.elementHeightCallback   += GetElementHeight;
				reorderableList.drawNoneElementCallback += DrawNoneElement;
			}

			if (!isFoldOut)
			{
				reorderableList.elementHeight = 0;
			}

			if (isFoldOut && !isValid)
			{
				DrawDuplicateKeyWarning(position, actualCount);

				position.y   += warningHeight;
				position.y   += spacingWarningToDictionary;
				propertySize += spacingWarningToDictionary;
			}

			reorderableList.DoList(position); // Draw the reorderable list

			propertySize += paddingAtEndOfProperty;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// Check if all the keys are distinct by comparing their value as a string
		/// </summary>
		/// <seealso cref="Structs.Utility.SerializableDictionary.SerializableKeyValuePair.key"/>
		/// <seealso cref="EditorUtils.GetValueString"/>
		private static bool IsValidDictionary(SerializedProperty dictionary, out int actualKeyCount)
		{
			List<string> pairLabels = GetKeyLabels(dictionary);

			actualKeyCount = pairLabels.Distinct().Count();
			return actualKeyCount == pairLabels.Count; // If the count of all distinct keys equals the count of all keys, then all keys are distinct
		}

		private void DrawHeader(Rect rect)
		{
			propertySize += headerHeight;

			Rect foldoutRect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);

			GUIContent label = currentLabel;

			if (!isValid)
			{
				label.text += " [CONFLICTS!]";
			}

			bool newFoldout = EditorGUI.Foldout(foldoutRect, isFoldOut, label, true);

			if (isFoldOut != newFoldout) // Foldout changed
			{
				reorderableList = null;
				isFoldOut       = newFoldout;
			}
		}

		private void DrawDuplicateKeyWarning(Rect position, int actualCount)
		{
			propertySize += warningHeight;

			position.height = warningHeight;
			EditorGUI.HelpBox(position, $"Duplicate keys found! These will be removed after serialization.\nActual Size: {actualCount}", MessageType.Warning);
		}

		private void DrawElement(Rect rect, int index, bool isactive, bool isfocused)
		{
			if (!isFoldOut)
			{
				return;
			}

			rect.y += paddingAtBeginOfElement;

			SerializedProperty keyValuePair = serializedDictionary.GetArrayElementAtIndex(index);
			SerializedProperty key = keyValuePair.FindPropertyRelative("key");
			SerializedProperty value = keyValuePair.FindPropertyRelative("value");

			Type[] genericArguments = fieldInfo.FieldType.GetGenericArguments();
			Type keyType = genericArguments[0];

			rect.height = EditorGUI.GetPropertyHeight(key, true);

			EditorGUI.PropertyField(rect, key, new GUIContent($"{EditorUtils.GetValueString(key, index, propertyType: keyType)} [{EditorUtils.GetTypeString(key)}]"), true);

			rect.y += rect.height + spacingBetweenPairValues; // Move the position down to beneath the last element + spacing

			rect.height = EditorGUI.GetPropertyHeight(value, true);

			EditorGUI.PropertyField(rect, value, new GUIContent($"Value [{EditorUtils.GetTypeString(value)}]"), true);

			propertySize += GetElementHeight(index) + 2; // + 2 because Unity adds 2 pixels padding to each element
		}

		private void DrawNoneElement(Rect rect)
		{
			if (!isFoldOut)
			{
				return;
			}

			propertySize += elementHeight; // GetElementHeight is not called for the NoneElement
			EditorGUI.LabelField(rect, EditorGUIUtility.TrTextContent("Dictionary is Empty"));
		}

		private float GetElementHeight(int index)
		{
			/*
			 * We do not update propertySize here because element height is cached and therefore not called every update
			 */

			if (!isFoldOut)
			{
				return 0;
			}

			float currentElementHeight = paddingAtBeginOfElement;

			SerializedProperty keyValuePair = serializedDictionary.GetArrayElementAtIndex(index);
			SerializedProperty key = keyValuePair.FindPropertyRelative("key");
			SerializedProperty value = keyValuePair.FindPropertyRelative("value");

			currentElementHeight += EditorGUI.GetPropertyHeight(key, true) + EditorGUI.GetPropertyHeight(value, true);

			currentElementHeight += spacingBetweenPairValues;
			currentElementHeight += paddingAtEndOfElement;

			return currentElementHeight;
		}

		private static List<string> GetKeyLabels(SerializedProperty dictionary)
		{
			List<string> labels = new List<string>();

			for (int i = 0; i < dictionary.arraySize; i++)
			{
				SerializedProperty keyValuePair = dictionary.GetArrayElementAtIndex(i);
				SerializedProperty key = keyValuePair.FindPropertyRelative("key"); // The key of a SerializableKeyValuePair<> is always called 'key'

				labels.Add(GetKeyLabel(key, i));
			}

			return labels;
		}

		private static string GetKeyLabel(SerializedProperty key, int index)
		{
			return EditorUtils.GetValueString(key, index, "Pair {index}");
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			base.GetPropertyHeight(property, label) + propertySize;
	}
}

/*
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utility.SerializableDictionary;
using VDFramework.Extensions;
using static Utility.EditorUtils;

namespace PropertyDrawers.Dictionary
{
	[CustomPropertyDrawer(typeof(SerializableDictionary<,>), false)]
	public class SerializableDictionaryDrawer : PropertyDrawer
	{
		// Constants, for consistent layout
		private const float spacingWarningToDictionary = 5.0f;
		private const float spacingDictionaryToPairs = 5.0f;
		private const float spacingLabelToPair = 0.0f;
		private const float pairIndent = 10.0f;
		private const float spacingBetweenPairValues = 2.0f;
		private const float spacingBetweenPairs = 0.0f;
		private const float paddingAtEndOfProperty = 0.0f;

		private const float foldoutHeight = 20.0f;
		private const float warningHeight = 30.0f;

		// Instance variables, to allow variable size between properties
		private Vector2 origin;
		private float propertySize;
		private float xpos;
		private float ypos;
		private float maxWidth;

		// Foldouts for the foldout fields
		private bool foldoutDictionary;
		private bool[] foldouts;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			origin = new Vector2(position.x, position.y);

			propertySize = 0;

			xpos = origin.x;
			ypos = origin.y;
			maxWidth = position.width;

			DrawDictionary(property, property.displayName);

			propertySize += paddingAtEndOfProperty;

			EditorGUI.EndProperty();
		}

		private void DrawDictionary(SerializedProperty property, string dictionaryName)
		{
			if (IsFoldOut(ref foldoutDictionary, $"{dictionaryName}"))
			{
				SerializedProperty list = property.FindPropertyRelative("serializedDictionary");
				
				List<string> pairLabels = GetLabels(list, "key");
				int actualCount = pairLabels.Distinct().Count(); 
				bool conflicts = actualCount != pairLabels.Count;
				
				if (conflicts)
				{
					DrawWarning(actualCount);
				}
				
				DrawSizeField(list);
				ResizeFoldouts(list);

				ypos += spacingDictionaryToPairs;

				DrawKeyValueArray(list, "key", "value", DrawPair);

				// Size = Y pos of end - Y pos of beginning - spacing at end of last pair
				propertySize = ypos - origin.y - spacingBetweenPairs;
			}
		}

		private void DrawWarning(int actualCount)
		{
			Rect rect = new Rect(xpos, ypos, maxWidth - xpos, warningHeight);
			EditorGUI.HelpBox(rect, "Duplicate keys found! These will be removed after serialization.", MessageType.Warning);

			ypos += warningHeight;
			
			rect.y = ypos;
			rect.height = 20.0f;

			EditorGUI.LabelField(rect, $"Actual Size: {actualCount}");
			
			ypos += 20.0f;
			ypos += spacingWarningToDictionary;
		}

		private void ResizeFoldouts(SerializedProperty list)
		{
			if (foldouts == null)
			{
				foldouts = new bool[list.arraySize];
				return;
			}

			if (foldouts.Length != list.arraySize)
			{
				List<bool> temp = foldouts.ToList();
				temp.ResizeList(list.arraySize);
				foldouts = temp.ToArray();
			}
		}

		private void DrawSizeField(SerializedProperty list)
		{
			Rect sizeRect = new Rect(xpos + pairIndent, ypos, maxWidth - xpos, foldoutHeight);
			list.arraySize = Mathf.Clamp(EditorGUI.IntField(sizeRect, new GUIContent("Size"), list.arraySize), 0, int.MaxValue);

			ypos += foldoutHeight;
		}

		private void DrawPair(int index, SerializedProperty key, SerializedProperty value)
		{
			if (IsFoldOut(ref foldouts[index], GetPairLabel(key, index).ReplaceUnderscoreWithSpace()))
			{
				ypos += spacingLabelToPair;
				DrawVariable(key, new GUIContent($"Key [{GetTypeString(key)}]"));

				ypos += spacingBetweenPairValues;
				DrawVariable(value, new GUIContent($"Value [{GetTypeString(value)}]"));
			}

			ypos += spacingBetweenPairs;
		}

		private void DrawVariable(SerializedProperty value, GUIContent label)
		{
			float xPosition = xpos + pairIndent;
			float height = EditorGUI.GetPropertyHeight(value, true);

			Rect rect = new Rect(xPosition, ypos, maxWidth - xPosition, height);
			EditorGUI.PropertyField(rect, value, label, true);

			ypos += height;
		}

		/// <summary>
		/// Creates a foldout label
		/// </summary>
		private bool IsFoldOut(ref bool foldout, string label = "")
		{
			Rect rect = new Rect(xpos, ypos, maxWidth - xpos, foldoutHeight);
			foldout = EditorGUI.Foldout(rect, foldout, label);

			ypos += foldoutHeight;

			return foldout;
		}

		private static List<string> GetLabels(SerializedProperty list, string keyName)
		{
			List<string> labels = new List<string>();
			
			for (int i = 0; i < list.arraySize; i++)
			{
				SerializedProperty keyValuePair = list.GetArrayElementAtIndex(i);
				SerializedProperty key = keyValuePair.FindPropertyRelative(keyName);
				
				labels.Add(GetPairLabel(key, i));
			}

			return labels;
		}

		private static string GetPairLabel(SerializedProperty key, int index)
		{
			return GetValueString(key, index, "Pair {index}");
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			base.GetPropertyHeight(property, label) + propertySize;
	}
}		
/**/ // old dictionary drawer (does not use reorderable list)