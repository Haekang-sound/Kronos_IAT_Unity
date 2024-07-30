using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public abstract class LayerFeature : IDrawable
    {
        protected const int MaxLayersCount = 4;

        public abstract void FindProperties(MaterialProperty[] properties);

        public abstract void Draw(PropertiesEditor editor);

        public abstract void SetKeywords(Material material);
    }
}