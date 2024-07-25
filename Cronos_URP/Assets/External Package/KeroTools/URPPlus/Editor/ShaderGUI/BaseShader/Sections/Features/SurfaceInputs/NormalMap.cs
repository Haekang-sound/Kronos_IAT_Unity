using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class NormalMap : IDrawable
    {
        private static readonly int NormalMapID = Shader.PropertyToID("_NormalMap");
        
        protected MaterialProperty NormalMapProperty;
        protected MaterialProperty NormalMapScaleProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            NormalMapProperty = PropertyFinder.FindOptionalProperty("_NormalMap", properties);
            NormalMapScaleProperty = PropertyFinder.FindOptionalProperty("_NormalScale", properties);
        }

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawNormalTexture(SurfaceInputsStyles.NormalMap, NormalMapProperty, NormalMapScaleProperty);

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(NormalMapID))
                CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture(NormalMapID));
        }
    }
}