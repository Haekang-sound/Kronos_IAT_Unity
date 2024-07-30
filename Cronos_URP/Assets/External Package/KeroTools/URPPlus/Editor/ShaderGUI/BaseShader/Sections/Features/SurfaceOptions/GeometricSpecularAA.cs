using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class GeometricSpecularAA : IDrawable
    {
        private static readonly int EnableGeometricSpecularAaID = Shader.PropertyToID("_EnableGeometricSpecularAA");
        
        protected MaterialProperty EnableSpecularAAProperty;
        protected MaterialProperty SpecularAAScreenSpaceVarianceProperty;
        protected MaterialProperty SpecularAAThresholdProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            EnableSpecularAAProperty = GetProperty("_EnableGeometricSpecularAA");
            SpecularAAScreenSpaceVarianceProperty = GetProperty("_SpecularAAScreenSpaceVariance");
            SpecularAAThresholdProperty = GetProperty("_SpecularAAThreshold");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var isEnabled = editor.DrawToggle(SurfaceOptionsStyles.GeometricSpecularAA, EnableSpecularAAProperty);
            
            if (!isEnabled)
                return;

            editor.DrawIndented(() =>
            {
                DrawScreenSpaceVariance(editor);
                DrawThreshold(editor);
            });
        }

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(EnableGeometricSpecularAaID)) 
                return;
            
            var state = EnableSpecularAAProperty.floatValue > 0.5f;
            CoreUtils.SetKeyword(material, "_ENABLE_GEOMETRIC_SPECULAR_AA", state);
        }

        protected virtual void DrawThreshold(PropertiesEditor editor) => 
            editor.DrawSlider(SurfaceOptionsStyles.SpecularAAThreshold, SpecularAAThresholdProperty);

        protected virtual void DrawScreenSpaceVariance(PropertiesEditor editor) => 
            editor.DrawSlider(SurfaceOptionsStyles.SpecularAAScreenSpaceVariance, SpecularAAScreenSpaceVarianceProperty);
    }
}