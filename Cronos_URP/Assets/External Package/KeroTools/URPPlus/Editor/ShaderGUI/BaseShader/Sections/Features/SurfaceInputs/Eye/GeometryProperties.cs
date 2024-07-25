using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye
{
    public class GeometryProperties : IDrawable
    {
        protected MaterialProperty MeshScaleProperty;

        public void FindProperties(MaterialProperty[] properties) => 
            MeshScaleProperty = PropertyFinder.FindOptionalProperty("_MeshScale", properties);

        public void Draw(PropertiesEditor editor) => 
            editor.DrawMinFloat(new GUIContent("Mesh Scale"), MeshScaleProperty, 0.0f);

        public void SetKeywords(Material material)
        {
        }
    }
}