/**
EnumLabel.cs
Copyright (c) 2016 Hassaku
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field)]
public class EnumLabelAttribute : PropertyAttribute{

	public string[] EnumNames { get; private set; }

	public EnumLabelAttribute(Type enumType) {
		Assert.IsTrue(enumType.IsEnum, "[EnumLabel] type of attribute parameter is not enum.");
		EnumNames = Enum.GetNames(enumType);
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumLabelAttribute))]
public class EnumLabelDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EnumLabelAttribute attr = attribute as EnumLabelAttribute;
		Match match = new Regex(@"Element (?<index>\d+)").Match(label.text);
		if(!match.Success){
			EditorGUI.PropertyField(position, property, label);
			return;
		}
		int index = int.Parse(match.Groups["index"].Value);

		if (index < attr.EnumNames.Length) {
			EditorGUI.PropertyField(position, property, new GUIContent(attr.EnumNames[index]));
		}
		else {
			EditorGUI.PropertyField(position, property, label);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUI.GetPropertyHeight(property);
	}
}
#endif