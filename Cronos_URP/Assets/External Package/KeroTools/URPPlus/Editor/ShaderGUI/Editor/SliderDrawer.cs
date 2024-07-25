using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class SliderDrawer
    {
        private readonly PropertiesEditorUtils _propertyUtils;

        public SliderDrawer(PropertiesEditorUtils propertyUtils) =>
            _propertyUtils = propertyUtils;
        
        public void DrawSlider(GUIContent label, MaterialProperty property) => 
            DrawSlider(label, property, property.rangeLimits.x, property.rangeLimits.y);
        
        public void DrawClampedSlider(GUIContent label, MaterialProperty property) => 
            DrawSlider(label, property, 0.0f, 1.0f);
        
        public void DrawSlider(GUIContent label, MaterialProperty property, float min, float max)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUILayout.Slider(label, property.floatValue, min, max);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = newValue;
            });
        }

        public void DrawIntSlider(GUIContent label, MaterialProperty property)
        {
            var limits = property.rangeLimits;
            DrawIntSlider(label, property, (int)limits.x, (int)limits.y);
        }
        
        public void DrawIntSlider(GUIContent label, MaterialProperty property, int min, int max)
        {
            _propertyUtils.ContainProperty(property, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;
                var newValue = EditorGUI.IntSlider(_propertyUtils.GetRect(property), label, (int)property.floatValue, min, max);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = newValue;
            });
        }

        public void MinMaxShaderProperty(GUIContent label, MaterialProperty min, MaterialProperty max) =>
            MinMaxShaderProperty(label, min, max, 0.0f, 1.0f);
        
        public void MinMaxShaderProperty(GUIContent label, MaterialProperty min, MaterialProperty max, float minLimit, float maxLimit)
        {
            _propertyUtils.ContainProperty(min, () =>
            {
                _propertyUtils.ContainProperty(max, () =>
                {
                    var minValue = min.floatValue;
                    var maxValue = max.floatValue;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.MinMaxSlider(label, ref minValue, ref maxValue, minLimit, maxLimit);
                    if (EditorGUI.EndChangeCheck())
                    {
                        min.floatValue = minValue;
                        max.floatValue = maxValue;
                    }
                });
            });
        }

        public void MinMaxShaderPropertyXY(GUIContent label, MaterialProperty remapProp, float minLimit, float maxLimit)
        {
            var remap = remapProp.vectorValue;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider(label, ref remap.x, ref remap.y, minLimit, maxLimit);
            if (EditorGUI.EndChangeCheck())
                remapProp.vectorValue = remap;
        }

        public void MinMaxShaderPropertyZW(GUIContent label, MaterialProperty remapProp, float minLimit, float maxLimit)
        {
            var remap = remapProp.vectorValue;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider(label, ref remap.z, ref remap.w, minLimit, maxLimit);
            if (EditorGUI.EndChangeCheck())
                remapProp.vectorValue = remap;
        }
    }
}