using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class PopupDrawer
    {
        private readonly PropertiesEditorUtils _propertyUtils;

        public PopupDrawer(PropertiesEditorUtils propertyUtils) =>
            _propertyUtils = propertyUtils;

        public int DrawPopup(GUIContent label, MaterialProperty property, string[] displayedOptions)
        {
            var value = 0;
            
            _propertyUtils.ContainProperty(property, () =>
            {
                value = (int)property.floatValue;
                
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.Popup(label, value, displayedOptions);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck() && (newValue != value || property.hasMixedValue))
                    property.floatValue = value = newValue;
            });

            return value;
        }
        
        public bool DrawBoolPopup(GUIContent label, MaterialProperty property, string[] displayedOptions)
        {
            if (displayedOptions.Length != 2)
            {
                EditorGUILayout.HelpBox("The displayedOptions array should contain exactly two options.", MessageType.Error);
                return false;
            }
            
            var value = DrawPopup(label, property, displayedOptions);

            return value != 0;
        }
    }
}