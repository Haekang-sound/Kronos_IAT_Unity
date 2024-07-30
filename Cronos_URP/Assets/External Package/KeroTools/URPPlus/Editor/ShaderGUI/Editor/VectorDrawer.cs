using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class VectorDrawer
    {
        private readonly PropertiesEditorUtils _propertyUtils;
        
        public VectorDrawer(PropertiesEditorUtils propertyUtils) => 
            _propertyUtils = propertyUtils;

        public void DrawFloat(GUIContent label, MaterialProperty property)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.FloatField(label, property.floatValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = newValue;
            });
        }
        
        public void DrawMinFloat(GUIContent label, MaterialProperty property, float min)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.FloatField(label, property.floatValue);
                newValue = Mathf.Max(min, newValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = newValue;
            });
        }
        
        public void DrawVector2(GUIContent label, MaterialProperty property)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.Vector2Field(label, property.vectorValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.vectorValue = newValue;
            });
        }
        
        public void DrawVector3(GUIContent label, MaterialProperty property)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.Vector3Field(label, property.vectorValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.vectorValue = newValue;
            });
        }
        
        public void DrawVector4(GUIContent label, MaterialProperty property)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.Vector4Field(label, property.vectorValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.vectorValue = newValue;
            });
        }
    }
}