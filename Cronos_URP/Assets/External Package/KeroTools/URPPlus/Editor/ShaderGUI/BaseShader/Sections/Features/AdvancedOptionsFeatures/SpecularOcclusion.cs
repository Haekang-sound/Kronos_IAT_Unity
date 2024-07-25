using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class SpecularOcclusion : IDrawable
    {
        private static readonly int SpecularOcclusionModeID = Shader.PropertyToID("_SpecularOcclusionMode");

        protected readonly string[] SpecularOcclusionOptions = Enum.GetNames(typeof(SpecularOcclusionMode));
        public MaterialProperty GIOcclusionBiasProperty;
        public MaterialProperty SpecularOcclusionModeProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            SpecularOcclusionModeProperty = PropertyFinder.FindOptionalProperty("_SpecularOcclusionMode", properties);
            GIOcclusionBiasProperty = PropertyFinder.FindOptionalProperty("_GIOcclusionBias", properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            DrawSpecularOcclusion(editor);
            editor.DrawIndented(() => DrawGIOcclusion(editor));
        }

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(SpecularOcclusionModeID)) 
                return;
            
            var specularOcclusionModeEnum = (SpecularOcclusionMode)material.GetFloat(SpecularOcclusionModeID);
            switch (specularOcclusionModeEnum)
            {
                case ShaderGUI.SpecularOcclusionMode.Off:
                    material.DisableKeyword("_AO_SPECULAR_OCCLUSION");
                    material.DisableKeyword("_BENTNORMAL_SPECULAR_OCCLUSION");
                    material.DisableKeyword("_GI_SPECULAR_OCCLUSION");
                    break;
                case ShaderGUI.SpecularOcclusionMode.FromAmbientOcclusion:
                    material.DisableKeyword("_BENTNORMAL_SPECULAR_OCCLUSION");
                    material.DisableKeyword("_GI_SPECULAR_OCCLUSION");
                    material.EnableKeyword("_AO_SPECULAR_OCCLUSION");
                    break;
                case ShaderGUI.SpecularOcclusionMode.FromBentNormals:
                    material.DisableKeyword("_AO_SPECULAR_OCCLUSION");
                    material.DisableKeyword("_GI_SPECULAR_OCCLUSION");
                    material.EnableKeyword("_BENTNORMAL_SPECULAR_OCCLUSION");
                    break;
                case ShaderGUI.SpecularOcclusionMode.FromGI:
                    material.DisableKeyword("_AO_SPECULAR_OCCLUSION");
                    material.DisableKeyword("_BENTNORMAL_SPECULAR_OCCLUSION");
                    material.EnableKeyword("_GI_SPECULAR_OCCLUSION");
                    break;
            }
        }

        protected virtual void DrawSpecularOcclusion(PropertiesEditor editor)
        {
            editor.DrawPopup(AdvancedOptionsStyles.SpecularOcclusionMode, SpecularOcclusionModeProperty,
                SpecularOcclusionOptions);
        }

        protected virtual void DrawGIOcclusion(PropertiesEditor editor)
        {
            var isFromGI = (SpecularOcclusionMode)SpecularOcclusionModeProperty.floatValue == ShaderGUI.SpecularOcclusionMode.FromGI;

            if (!isFromGI)
                return;

            editor.DrawSlider(AdvancedOptionsStyles.GIOcclusionBias, GIOcclusionBiasProperty);
        }
    }
}