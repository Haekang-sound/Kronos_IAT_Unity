using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class ToggleDrawer
    {
        private readonly PropertiesEditorUtils _propertyUtils;
        
        public const float BoolTolerance = 0.5f;

        public ToggleDrawer(PropertiesEditorUtils propertyUtils) =>
            _propertyUtils = propertyUtils;
        
        public bool DrawToggle(GUIContent label, MaterialProperty property)
        {
            var newValue = false;
            
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var currentValue = Mathf.Abs(property.floatValue) >= BoolTolerance;
                newValue = EditorGUILayout.Toggle(label, currentValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = newValue ? 1.0f : 0.0f;
            });
            
            return newValue;
        }
    }
}