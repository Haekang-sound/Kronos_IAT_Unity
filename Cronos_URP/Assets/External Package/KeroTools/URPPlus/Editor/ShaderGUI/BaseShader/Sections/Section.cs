using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public abstract class Section : IDrawable
    {
        public GUIContent Label;
        public Expandable Expandable;
        public bool IsRendered = true;

        public abstract void FindProperties(MaterialProperty[] properties);

        public abstract void Draw(PropertiesEditor editor);

        public abstract void SetKeywords(Material material);
    }
}