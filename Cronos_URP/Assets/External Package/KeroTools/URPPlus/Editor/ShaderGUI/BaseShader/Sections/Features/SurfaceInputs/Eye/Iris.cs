using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye
{
    public class Iris : IDrawable
    {
        private static readonly int IrisNormalID = Shader.PropertyToID("_IrisNormalMap");
        protected MaterialProperty CorneaSmoothnessProperty;
        protected MaterialProperty IrisClampColorProperty;
        protected MaterialProperty IrisMapProperty;
        protected MaterialProperty IrisNormalProperty;
        protected MaterialProperty IrisNormalIntensityProperty;
        protected MaterialProperty IrisPositionOffsetProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            IrisMapProperty = GetProperty("_IrisMap");
            IrisClampColorProperty = GetProperty("_IrisClampColor");
            IrisNormalProperty = GetProperty("_IrisNormalMap");
            IrisNormalIntensityProperty = GetProperty("_IrisNormalScale");
            IrisPositionOffsetProperty = GetProperty("_PositionOffset");
            CorneaSmoothnessProperty = GetProperty("_CorneaSmoothness");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(new GUIContent("Iris Map"), IrisMapProperty);
            editor.DrawColor(new GUIContent("Iris Clamp Color"), IrisClampColorProperty);
            editor.DrawTexture(new GUIContent("Iris NormalMap"), IrisNormalProperty, IrisNormalIntensityProperty);
            editor.DrawVector3(new GUIContent("Iris Position Offset"), IrisPositionOffsetProperty);
            editor.DrawSlider(new GUIContent("Cornea Smoothness"), CorneaSmoothnessProperty);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(IrisNormalID))
                CoreUtils.SetKeyword(material, "_IRIS_NORMALMAP", material.GetTexture(IrisNormalID));
        }
    }
}