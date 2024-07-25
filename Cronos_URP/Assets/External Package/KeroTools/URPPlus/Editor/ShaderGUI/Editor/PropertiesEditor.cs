using System;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class PropertiesEditor
    {
        private readonly MaterialEditor _materialEditor;
        private readonly PropertiesEditorUtils _propertyUtils;
        
        private readonly ToggleDrawer _toggleDrawer;
        private readonly SliderDrawer _sliderDrawer;
        private readonly PopupDrawer _popupDrawer;
        private readonly VectorDrawer _vectorDrawer;
        private readonly TextureDrawer _textureDrawer;
        
        public PropertiesEditor(MaterialEditor materialEditor)
        {
            _materialEditor = materialEditor;
            _propertyUtils = new PropertiesEditorUtils();
            _toggleDrawer = new ToggleDrawer(_propertyUtils);
            _sliderDrawer = new SliderDrawer(_propertyUtils);
            _popupDrawer = new PopupDrawer(_propertyUtils);
            _vectorDrawer = new VectorDrawer(_propertyUtils);
            _textureDrawer = new TextureDrawer(materialEditor, _propertyUtils);
        }

        public Object[] GetTargets() => 
            _materialEditor.targets;

        public bool DrawToggle(GUIContent label, MaterialProperty property) =>
            _toggleDrawer.DrawToggle(label, property);
        
        public void DrawSlider(GUIContent label, MaterialProperty property) =>
            _sliderDrawer.DrawSlider(label, property);
        
        public void DrawClampedSlider(GUIContent label, MaterialProperty property) =>
            _sliderDrawer.DrawClampedSlider(label, property);
        
        public void DrawSlider(GUIContent label, MaterialProperty property, float min, float max) =>
            _sliderDrawer.DrawSlider(label, property, min, max);

        public void DrawIntSlider(GUIContent label, MaterialProperty property) =>
            _sliderDrawer.DrawIntSlider(label, property);
        
        public void DrawIntSlider(GUIContent label, MaterialProperty property, int min, int max) =>
            _sliderDrawer.DrawIntSlider(label, property, min, max);
        
        public void MinMaxShaderProperty(GUIContent label, MaterialProperty min, MaterialProperty max) =>
            _sliderDrawer.MinMaxShaderProperty(label, min, max, 0.0f, 1.0f);
        
        public void MinMaxShaderProperty(GUIContent label, MaterialProperty min, MaterialProperty max, float minLimit, float maxLimit) =>
            _sliderDrawer.MinMaxShaderProperty(label, min, max, minLimit, maxLimit);
        
        public void MinMaxShaderPropertyXY(GUIContent label, MaterialProperty remapProp) =>
            _sliderDrawer.MinMaxShaderPropertyXY(label, remapProp, 0.0f, 1.0f);
        
        public void MinMaxShaderPropertyXY(GUIContent label, MaterialProperty remapProp, float minLimit, float maxLimit) =>
            _sliderDrawer.MinMaxShaderPropertyXY(label, remapProp, minLimit, maxLimit);
        
        public void MinMaxShaderPropertyZW(GUIContent label, MaterialProperty remapProp) =>
            _sliderDrawer.MinMaxShaderPropertyZW(label, remapProp, 0.0f, 1.0f);
        
        public void MinMaxShaderPropertyZW(GUIContent label, MaterialProperty remapProp, float minLimit, float maxLimit) =>
            _sliderDrawer.MinMaxShaderPropertyZW(label, remapProp, minLimit, maxLimit);

        public int DrawPopup(GUIContent label, MaterialProperty property, string[] displayedOptions) =>
            _popupDrawer.DrawPopup(label, property, displayedOptions);
        
        public bool DrawBoolPopup(GUIContent label, MaterialProperty property, string[] displayedOptions) =>
            _popupDrawer.DrawBoolPopup(label, property, displayedOptions);

        public void DrawFloat(GUIContent label, MaterialProperty property) =>
            _vectorDrawer.DrawFloat(label, property);
        
        public void DrawMinFloat(GUIContent label, MaterialProperty property, float min) =>
            _vectorDrawer.DrawMinFloat(label, property, min);
        
        public void DrawVector2(GUIContent label, MaterialProperty property) => 
            _vectorDrawer.DrawVector2(label, property);
        
        public void DrawVector3(GUIContent label, MaterialProperty property) => 
            _vectorDrawer.DrawVector3(label, property);

        public void DrawVector4(GUIContent label, MaterialProperty property) => 
            _vectorDrawer.DrawVector4(label, property);

        public void DrawColor(GUIContent label, MaterialProperty property) =>
            _textureDrawer.DrawColor(label, property);

        public void DrawTexture(GUIContent label, MaterialProperty textureProperty) =>
            _textureDrawer.DrawTexture(label, textureProperty);
        
        public void DrawTexture(GUIContent label, MaterialProperty textureProperty, MaterialProperty secondProperty) =>
            _textureDrawer.DrawTexture(label, textureProperty, secondProperty);
        
        public void DrawNormalTexture(GUIContent label, MaterialProperty normalMap, MaterialProperty normalMapScale = null) =>
            _textureDrawer.DrawNormalTexture(label, normalMap, normalMapScale);
        
        public void DrawTextureWithHDRColor(GUIContent label, MaterialProperty textureProperty, MaterialProperty colorProperty) =>
            _textureDrawer.DrawTextureWithHDRColor(label, textureProperty, colorProperty);
        
        public void DrawTextureScaleOffset(MaterialProperty textureProperty) =>
            _textureDrawer.DrawTextureScaleOffset(textureProperty);

        public void DrawEmissionGIMode() =>
            _materialEditor.LightmapEmissionProperty();
        
        public void DrawGPUInstancing() =>
            _materialEditor.EnableInstancingField();
        
        public void DrawIndented(Action drawCall) => 
            _propertyUtils.DrawIndented(drawCall);
        
        public void DrawIndented(Action drawCall, uint level = 0) => 
            _propertyUtils.DrawIndented(drawCall, level);
    }
}