using KeroTools.URPPlus.Runtime;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.Drawing
{
    public class FloatDrawing
    {
        private readonly KeroEditorUtils _editorUtils;

        public FloatDrawing(KeroEditorUtils editorUtils) =>
            _editorUtils = editorUtils;
        
        public void DrawFloat(GUIContent label, SerializedProperty property, Vector2 minMax, int indentLevel = 0)
        {
            if (property is null)
                return;

            _editorUtils.DrawIndented(indentLevel, () =>
            {
                EditorGUI.BeginChangeCheck();
                var newValue = Mathf.Clamp(EditorGUILayout.FloatField(label, property.floatValue), minMax.x, minMax.y);
                if (EditorGUI.EndChangeCheck())
                {
                    property.floatValue = newValue;
                }
            });
        }
        
        public void DrawSlider(GUIContent label, SerializedProperty property, Vector2 minMax, int indentLevel = 0)
        {
            if (property is null)
                return;

            _editorUtils.DrawIndented(indentLevel, () =>
            {
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUILayout.Slider(label, property.floatValue, minMax.x, minMax.y);
                if (EditorGUI.EndChangeCheck())
                {
                    property.floatValue = newValue;
                }
            });
        }

        public void DrawFloatToggle(GUIContent label, SerializedProperty property, string keyword, int indentLevel = 0)
        {
            if (property is null)
                return;

            _editorUtils.DrawIndented(indentLevel, () =>
            {
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUILayout.Toggle(label, property.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.boolValue = newValue;
                    URPPlusSettings.SetKeyword(keyword, newValue);
                }
            });
        }
    }
}