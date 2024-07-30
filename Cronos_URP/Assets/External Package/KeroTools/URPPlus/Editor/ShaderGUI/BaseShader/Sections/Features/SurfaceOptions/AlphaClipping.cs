using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class AlphaClipping : IDrawable
    {
        private static readonly int AlphaCutoffEnableID = Shader.PropertyToID("_AlphaCutoffEnable");
        private static readonly int UseShadowThresholdID = Shader.PropertyToID("_UseShadowThreshold");
        
        protected MaterialProperty AlphaCutoffProperty;
        protected MaterialProperty AlphaCutoffEnableProperty;
        protected MaterialProperty AlphaCutoffShadowProperty;
        protected MaterialProperty AlphaToMaskProperty;
        protected MaterialProperty UseShadowThresholdProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            AlphaCutoffEnableProperty = GetProperty("_AlphaCutoffEnable");
            UseShadowThresholdProperty = GetProperty("_UseShadowThreshold");
            AlphaCutoffProperty = GetProperty("_AlphaCutoff");
            AlphaCutoffShadowProperty = GetProperty("_AlphaCutoffShadow");
            AlphaToMaskProperty = GetProperty("_AlphaToMask");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var alphaCutoffEnabled = DrawAlphaCutoff(editor);

            if (!alphaCutoffEnabled)
                return;

            editor.DrawIndented(() =>
            {
                DrawShadowAlphaCutoff(editor);
                DrawAlphaToMask(editor);
            });
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(AlphaCutoffEnableID))
            {
                var alphaCutoffState = AlphaCutoffEnableProperty.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, "_ALPHATEST_ON", alphaCutoffState);
            }

            if (material.HasProperty(UseShadowThresholdID))
            {
                var shadowCutoffState = UseShadowThresholdProperty.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, "_SHADOW_CUTOFF", shadowCutoffState);
            }
        }

        protected virtual bool DrawAlphaCutoff(PropertiesEditor editor)
        {
            return DrawCutoff(editor, SurfaceOptionsStyles.AlphaCutoffEnable, AlphaCutoffEnableProperty,
                () => DrawAlphaThreshold(editor));
        }

        protected virtual bool DrawShadowAlphaCutoff(PropertiesEditor editor)
        {
            return DrawCutoff(editor, SurfaceOptionsStyles.UseShadowThreshold, UseShadowThresholdProperty,
                () => DrawShadowAlphaThreshold(editor));
        }

        protected virtual void DrawAlphaThreshold(PropertiesEditor editor) => 
            editor.DrawSlider(SurfaceOptionsStyles.AlphaCutoff, AlphaCutoffProperty);

        protected virtual void DrawShadowAlphaThreshold(PropertiesEditor editor) => 
            editor.DrawSlider(SurfaceOptionsStyles.AlphaCutoffShadow, AlphaCutoffShadowProperty);

        protected virtual void DrawAlphaToMask(PropertiesEditor editor) => 
            editor.DrawToggle(SurfaceOptionsStyles.AlphaToMask, AlphaToMaskProperty);

        private bool DrawCutoff(PropertiesEditor editor, GUIContent label, MaterialProperty property, Action drawCall)
        {
            var isEnabled = editor.DrawToggle(label, property);

            if (!isEnabled)
                return false;

            editor.DrawIndented(drawCall);

            return true;
        }
    }
}