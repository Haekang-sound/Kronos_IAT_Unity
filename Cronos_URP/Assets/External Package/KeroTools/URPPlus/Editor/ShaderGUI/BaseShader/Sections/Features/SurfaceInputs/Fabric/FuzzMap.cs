using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Fabric
{
    public class FuzzMap : IDrawable
    {
        private static readonly int FuzzMapMapID = Shader.PropertyToID("_FuzzMap");
        protected MaterialProperty FuzzIntensityProperty;
        protected MaterialProperty FuzzMapProperty;
        protected MaterialProperty FuzzScaleProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            FuzzMapProperty = GetProperty("_FuzzMap");
            FuzzScaleProperty = GetProperty("_FuzzScale");
            FuzzIntensityProperty = GetProperty("_FuzzIntensity");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(new GUIContent("Fuzz Map"), FuzzMapProperty);
            editor.DrawMinFloat(new GUIContent("Fuzz Scale"), FuzzScaleProperty, 0.0f);
            editor.DrawSlider(new GUIContent("Fuzz Intensity"), FuzzIntensityProperty);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(FuzzMapMapID))
                CoreUtils.SetKeyword(material, "_FUZZMAP", material.GetTexture(FuzzMapMapID));
        }
    }
}