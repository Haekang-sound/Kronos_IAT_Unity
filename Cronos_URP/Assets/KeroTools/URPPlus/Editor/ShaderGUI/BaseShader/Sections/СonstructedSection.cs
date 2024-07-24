using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class СonstructedSection : Section
    {
        public IDrawable[] Features;

        public СonstructedSection(IDrawable[] features)
        {
            Features = features;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            foreach (var feature in Features)
                feature.FindProperties(properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            foreach (var feature in Features)
                feature.Draw(editor);
        }

        public override void SetKeywords(Material material)
        {
            foreach (var feature in Features)
                feature.SetKeywords(material);
        }
    }
}