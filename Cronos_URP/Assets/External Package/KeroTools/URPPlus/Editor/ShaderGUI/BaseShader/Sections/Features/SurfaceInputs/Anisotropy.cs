using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class Anisotropy : IDrawable
    {
        private static readonly int MaterialType = Shader.PropertyToID("_MaterialType");
        private static readonly int AnisotropyMap = Shader.PropertyToID("_AnisotropyMap");
        private static readonly int TangentMap = Shader.PropertyToID("_TangentMap");
        private readonly Material _material;
        
        protected MaterialProperty AnisotropyProperty;
        protected MaterialProperty AnisotropyMapProperty;
        protected MaterialProperty TangentMapProperty;

        public Anisotropy(Material material) => 
            _material = material;

        public void FindProperties(MaterialProperty[] properties)
        {
            TangentMapProperty = GetProperty("_TangentMap");
            AnisotropyProperty = GetProperty("_Anisotropy");
            AnisotropyMapProperty = GetProperty("_AnisotropyMap");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            var materialType = (MaterialTypeMode)_material.GetFloat(MaterialType);

            if (materialType != MaterialTypeMode.Anisotropy)
                return;

            DrawProperties(editor);
        }

        public virtual void SetKeywords(Material material)
        {
            if (material.HasProperty(TangentMap))
                CoreUtils.SetKeyword(material, "_TANGENTMAP", material.GetTexture(TangentMap));

            if (material.HasProperty(AnisotropyMap))
                CoreUtils.SetKeyword(material, "_ANISOTROPYMAP", material.GetTexture(AnisotropyMap));
        }

        protected virtual void DrawProperties(PropertiesEditor editor)
        {
            editor.DrawTexture(AnisotropyStyles.TangentMap, TangentMapProperty);
            editor.DrawSlider(AnisotropyStyles.Anisotropy, AnisotropyProperty);
            editor.DrawTexture(AnisotropyStyles.AnisotropyMap, AnisotropyMapProperty);
        }
    }
}