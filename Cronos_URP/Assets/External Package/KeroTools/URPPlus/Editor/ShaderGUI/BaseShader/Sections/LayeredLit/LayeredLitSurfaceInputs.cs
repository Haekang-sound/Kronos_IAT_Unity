using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit
{
    public class LayeredLitSurfaceInputs : IDrawable
    {
        protected MaterialProperty LayerCountProperty;
        protected MaterialProperty LayerMaskProperty;
        protected MaterialProperty VertexColorModeProperty;
        protected MaterialProperty UseMainLayerInfluenceProperty;
        protected MaterialProperty UseHeightBasedBlendingProperty;
        protected MaterialProperty HeightTransitionProperty;

        protected readonly string[] VertexColorModeOptions = Enum.GetNames(typeof(VertexColorMode));

        private static readonly int LayerCountID = Shader.PropertyToID("_LayerCount");
        private static readonly int VertexColorModeID = Shader.PropertyToID("_VertexColorMode");
        private static readonly int UseMainLayerInfluenceID = Shader.PropertyToID("_UseMainLayerInfluence");
        private static readonly int UseHeightBasedBlendID = Shader.PropertyToID("_UseHeightBasedBlend");

        private static readonly int OpacityAsDensity1ID = Shader.PropertyToID("_OpacityAsDensity1");
        private static readonly int OpacityAsDensity2ID = Shader.PropertyToID("_OpacityAsDensity2");
        private static readonly int OpacityAsDensity3ID = Shader.PropertyToID("_OpacityAsDensity3");

        private const string MainLayerInfluenceKeyword = "_MAIN_LAYER_INFLUENCE_MODE";
        private const string HeightBasedBlendKeyword = "_HEIGHT_BASED_BLEND";
        private const string LayeredLit4LayersKeyword = "_LAYEREDLIT_4_LAYERS";
        private const string LayeredLit3LayersKeyword = "_LAYEREDLIT_3_LAYERS";
        private const string VertexColorMultiplyKeyword = "_LAYER_MASK_VERTEX_COLOR_MUL";
        private const string VertexColorAddKeyword = "_LAYER_MASK_VERTEX_COLOR_ADD";
        private const string DensityModeKeyword = "_DENSITY_MODE";

        public void FindProperties(MaterialProperty[] properties)
        {
            LayerCountProperty = GetProperty("_LayerCount");
            LayerMaskProperty = GetProperty("_LayerMaskMap");
            VertexColorModeProperty = GetProperty("_VertexColorMode");
            UseMainLayerInfluenceProperty = GetProperty("_UseMainLayerInfluence");
            UseHeightBasedBlendingProperty = GetProperty("_UseHeightBasedBlend");
            HeightTransitionProperty = GetProperty("_HeightTransition");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawIntSlider(LayeredStyles.LayerCount, LayerCountProperty, 2, 4);
            editor.DrawTexture(LayeredStyles.LayerMaskMap, LayerMaskProperty);
            editor.DrawTextureScaleOffset(LayerMaskProperty);
            editor.DrawPopup(LayeredStyles.VertexColorMode, VertexColorModeProperty, VertexColorModeOptions);
            editor.DrawToggle(LayeredStyles.UseMainLayerInfluenceMode, UseMainLayerInfluenceProperty);

            DrawHeightBaseBlending(editor);
        }

        protected virtual void DrawHeightBaseBlending(PropertiesEditor editor)
        {
            editor.DrawToggle(LayeredStyles.UseHeightBasedBlend, UseHeightBasedBlendingProperty);
            editor.DrawIndented(() =>
            {
                if (UseHeightBasedBlendingProperty.floatValue > 0.5f)
                    editor.DrawSlider(LayeredStyles.HeightTransition, HeightTransitionProperty);
            });
        }

        public void SetKeywords(Material material)
        {
            SetLayerCountKeywords(material);
            SetVertexColorModeKeywords(material);

            if (material.HasProperty(UseMainLayerInfluenceID))
            {
                var mainInfluenceState = UseMainLayerInfluenceProperty.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, MainLayerInfluenceKeyword, mainInfluenceState);
            }

            if (material.HasProperty(UseHeightBasedBlendID))
            {
                var heightBasedBlendingState = UseHeightBasedBlendingProperty.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, HeightBasedBlendKeyword, heightBasedBlendingState);
            }

            SetDensityMode(material);
        }

        private void SetLayerCountKeywords(Material material)
        {
            if (material.HasProperty(LayerCountID))
            {
                var numLayers = (int)material.GetFloat(LayerCountID);

                CoreUtils.SetKeyword(material, LayeredLit4LayersKeyword, numLayers == 4);
                CoreUtils.SetKeyword(material, LayeredLit3LayersKeyword, numLayers == 3);
            }
            else
            {
                CoreUtils.SetKeyword(material, LayeredLit4LayersKeyword, false);
                CoreUtils.SetKeyword(material, LayeredLit3LayersKeyword, false);
            }
        }

        private void SetVertexColorModeKeywords(Material material)
        {
            if (material.HasProperty(VertexColorModeID))
            {
                var vcMode = (VertexColorMode)material.GetFloat(VertexColorModeID);

                CoreUtils.SetKeyword(material, VertexColorMultiplyKeyword, vcMode == ShaderGUI.VertexColorMode.Multiply);
                CoreUtils.SetKeyword(material, VertexColorAddKeyword, vcMode == ShaderGUI.VertexColorMode.Add);
            }
            else
            {
                CoreUtils.SetKeyword(material, VertexColorMultiplyKeyword, false);
                CoreUtils.SetKeyword(material, VertexColorAddKeyword, false);
            }
        }

        protected virtual void SetDensityMode(Material material)
        {
            if (!material.HasProperty(OpacityAsDensity1ID) && !material.HasProperty(OpacityAsDensity2ID) &&
                !material.HasProperty(OpacityAsDensity3ID)) 
                return;
            
            var opacityAsDensity1State = material.GetFloat(OpacityAsDensity1ID) > 0.5f;
            var opacityAsDensity2State = material.GetFloat(OpacityAsDensity2ID) > 0.5f;
            var opacityAsDensity3State = material.GetFloat(OpacityAsDensity3ID) > 0.5f;
            var opacityAsDensityState = opacityAsDensity1State || opacityAsDensity2State || opacityAsDensity3State;
            CoreUtils.SetKeyword(material, DensityModeKeyword, opacityAsDensityState);
        }
    }
}