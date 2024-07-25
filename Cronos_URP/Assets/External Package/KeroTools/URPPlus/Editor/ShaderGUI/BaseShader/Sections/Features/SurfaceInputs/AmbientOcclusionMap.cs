using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class AmbientOcclusionMap : IDrawable
    {
        protected MaterialProperty AmbientOcclusionMapProperty;
        protected MaterialProperty AORemapMaxProperty;
        protected MaterialProperty AORemapMinProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            AmbientOcclusionMapProperty = GetProperty("_AmbientOcclusionMap");
            AORemapMinProperty = GetProperty("_AORemapMin");
            AORemapMaxProperty = GetProperty("_AORemapMax");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(SurfaceInputsStyles.AOMap, AmbientOcclusionMapProperty);

            if (AmbientOcclusionMapProperty.textureValue is not null)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.AORemapping, AORemapMinProperty, AORemapMaxProperty);
        }

        public void SetKeywords(Material material)
        {
            CoreUtils.SetKeyword(material, "_AO_MAP", AmbientOcclusionMapProperty.textureValue is not null);
        }
    }
}