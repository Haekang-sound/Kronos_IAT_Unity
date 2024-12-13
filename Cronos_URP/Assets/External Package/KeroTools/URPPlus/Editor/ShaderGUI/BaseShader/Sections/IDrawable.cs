using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public interface IDrawable
    {
        public void FindProperties(MaterialProperty[] properties);

        public void Draw(PropertiesEditor editor);

        public void SetKeywords(Material material);
    }
}