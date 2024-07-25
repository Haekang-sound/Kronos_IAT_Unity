using KeroTools.URPPlus.Runtime;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.Drawing
{
    public class PopupDrawing
    {
        public void DrawBooleanPopup(GUIContent label, SerializedProperty property, string[] displayedOptions, string keyword)
        {
            if (property is null)
                return;

            if (displayedOptions.Length != 2)
            {
                EditorGUILayout.HelpBox("The displayedOptions array should contain exactly two options.", MessageType.Error);
                return;
            }

            EditorGUI.BeginChangeCheck();
            var selectedOptionIndex = (property.boolValue) ? 1 : 0;
            selectedOptionIndex = EditorGUILayout.Popup(label, selectedOptionIndex, displayedOptions);
            if (EditorGUI.EndChangeCheck())
            {
                property.boolValue = (selectedOptionIndex == 1);
                URPPlusSettings.SetKeyword(keyword, property.boolValue);
            }
        }
    }
}