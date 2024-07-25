using System;

using UnityEngine;
using UnityEditor;

using static KeroTools.URPPlus.Editor.ShaderGUI.Styles.SurfaceInputsStyles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class TextureDrawer
    {
        private readonly MaterialEditor _materialEditor;
        private readonly PropertiesEditorUtils _propertyUtils;

        public TextureDrawer(MaterialEditor materialEditor, PropertiesEditorUtils propertyUtils)
        {
            _materialEditor = materialEditor;
            _propertyUtils = propertyUtils;
        }

        public void DrawTexture(GUIContent label, MaterialProperty textureProperty)
        {
            if (textureProperty is null)
                return;

            _materialEditor.TexturePropertySingleLine(label, textureProperty);
        }
        
        public void DrawTexture(GUIContent label, MaterialProperty textureProperty, MaterialProperty secondProperty)
        {
            if (textureProperty is null)
                return;
            
            if (secondProperty is null)
                return;
            
            _materialEditor.TexturePropertySingleLine(label, textureProperty, secondProperty);
        }
        
        public void DrawTextureWithHDRColor(GUIContent label, MaterialProperty textureProperty, MaterialProperty colorProperty)
        {
            if (textureProperty is null)
                return;
            
            if (colorProperty is null)
                return;
            
            _materialEditor.TexturePropertyWithHDRColor(label, textureProperty, 
                colorProperty, false);
        }
        
        public void DrawNormalTexture(GUIContent label, MaterialProperty normalMap, MaterialProperty normalMapScale)
        {
            if (normalMap is null)
                return;
                
            var hasBumpMap = normalMap.textureValue is not null;
            var materialProperty = hasBumpMap ? normalMapScale : null;
            
            _materialEditor.TexturePropertySingleLine(label, normalMap, materialProperty);
            
            if (normalMapScale is null)
                return;
            
            var incorrectScale = Math.Abs(normalMapScale.floatValue - 1.0f) > 0.001f;
            if (incorrectScale && _propertyUtils.IsMobilePlatform())
                FixNormalScale(normalMapScale);
        }

        public void FixNormalScale(MaterialProperty normalMapScale)
        {
            var fixScale = _materialEditor.HelpBoxWithButton(BumpScaleNotSupported, FixNormal);
            
            if (fixScale)
                normalMapScale.floatValue = 1.0f;
        }
        
        public void DrawColor(GUIContent label, MaterialProperty colorProperty)
        {
            _propertyUtils.ContainProperty(colorProperty, () =>
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = colorProperty.hasMixedValue;
                var newValue = EditorGUI.ColorField(_propertyUtils.GetRect(colorProperty), label, colorProperty.colorValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                    colorProperty.colorValue = newValue;
            });
        }

        public void DrawTextureScaleOffset(MaterialProperty textureProperty)
        {
            if(textureProperty is null)
                return;
            
            _materialEditor.TextureScaleOffsetProperty(textureProperty);
        }
    }
}