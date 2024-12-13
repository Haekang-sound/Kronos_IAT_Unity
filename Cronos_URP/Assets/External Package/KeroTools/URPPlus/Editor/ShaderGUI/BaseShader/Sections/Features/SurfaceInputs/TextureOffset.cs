using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class TextureOffset : IDrawable
    {
        private readonly string _propertyName;
        protected MaterialProperty TexturePropertyProperty;

        public TextureOffset(string propertyName) => 
            _propertyName = propertyName;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            TexturePropertyProperty = PropertyFinder.FindOptionalProperty(_propertyName, properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawTextureScaleOffset(TexturePropertyProperty);

        public void SetKeywords(Material material)
        {
        }
    }
}