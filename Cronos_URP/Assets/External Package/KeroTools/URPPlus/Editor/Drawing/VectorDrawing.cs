using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.Drawing
{
    public class VectorDrawing
    {
        private readonly KeroEditorUtils _editorUtils;

        public VectorDrawing(KeroEditorUtils editorUtils) =>
            _editorUtils = editorUtils;

        public void DrawVector2(GUIContent label, SerializedProperty property, int indentLevel = 0)
        {
            if (property is null)
                return;
            
            _editorUtils.DrawIndented(indentLevel, () =>
            {
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUILayout.Vector2Field(label, property.vector2Value);
                if (EditorGUI.EndChangeCheck())
                    property.vector2Value = newValue;
            });
        }
        
        public void DrawColor(GUIContent label, SerializedProperty property)
        {
            if (property is not { propertyType: SerializedPropertyType.Color })
                return;
            
            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            var newValue = EditorGUILayout.ColorField(label, property.colorValue);
            EditorGUI.showMixedValue = false;

            if (EditorGUI.EndChangeCheck())
            {
                property.colorValue = newValue;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}