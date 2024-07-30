using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye
{
    public class Sclera : IDrawable
    {
        private static readonly int ScleraNormalID = Shader.PropertyToID("_ScleraNormalMap");
        protected MaterialProperty ScleraMapProperty;
        protected MaterialProperty ScleraNormalIntensityProperty;
        protected MaterialProperty ScleraNormalMapProperty;
        protected MaterialProperty ScleraSmoothnessProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            ScleraMapProperty = GetProperty("_ScleraMap");
            ScleraSmoothnessProperty = GetProperty("_ScleraSmoothness");
            ScleraNormalMapProperty = GetProperty("_ScleraNormalMap");
            ScleraNormalIntensityProperty = GetProperty("_ScleraNormalScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(new GUIContent("Sclera Map"), ScleraMapProperty);
            editor.DrawSlider(new GUIContent("Sclera Smoothness"), ScleraSmoothnessProperty);
            editor.DrawTexture(new GUIContent("Sclera Normal Map"), ScleraNormalMapProperty, ScleraNormalIntensityProperty);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(ScleraNormalID))
                CoreUtils.SetKeyword(material, "_SCLERA_NORMALMAP", material.GetTexture(ScleraNormalID));
        }
    }
}