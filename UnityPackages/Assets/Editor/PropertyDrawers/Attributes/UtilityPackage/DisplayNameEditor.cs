﻿using UnityEditor;
using UnityEngine;
using UtilityPackage.Attributes;
using VDFramework.Extensions;

namespace PropertyDrawers.Attributes.UtilityPackage
{
	[CustomPropertyDrawer(typeof(DisplayNameAttribute))]
	public class DisplayNameEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string name = attribute.ConvertTo<DisplayNameAttribute>().DisplayName;
			EditorGUI.PropertyField(position, property, new GUIContent(name));
		}
	}
}