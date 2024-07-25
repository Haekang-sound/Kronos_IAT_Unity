using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class DetailInputs : Section
    {
        protected MaterialProperty DetailMapProperty;
        protected MaterialProperty DetailAlbedoScaleProperty;
        protected MaterialProperty DetailNormalScaleProperty;
        protected MaterialProperty DetailSmoothnessScaleProperty;

        private static readonly int DetailMapID = Shader.PropertyToID("_DetailMap");

        public DetailInputs()
        {
            Label = DetailInputsStyles.Label;
            Expandable = Expandable.DetailInputs;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            DetailMapProperty = GetProperty("_DetailMap");
            DetailAlbedoScaleProperty = GetProperty("_DetailAlbedoScale");
            DetailNormalScaleProperty = GetProperty("_DetailNormalScale");
            DetailSmoothnessScaleProperty = GetProperty("_DetailSmoothnessScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            DrawDetailMap(editor);
            DrawDetailMapProperties(editor);
            editor.DrawTextureScaleOffset(DetailMapProperty);
        }

        private void DrawDetailMap(PropertiesEditor editor) => 
            editor.DrawTexture(DetailInputsStyles.DetailMap, DetailMapProperty);

        protected virtual void DrawDetailMapProperties(PropertiesEditor editor)
        {
            if (DetailMapProperty.textureValue is null)
                return;

            editor.DrawIndented(() =>
            {
                editor.DrawSlider(DetailInputsStyles.DetailAlbedoScale, DetailAlbedoScaleProperty);
                editor.DrawSlider(DetailInputsStyles.DetailNormalScale, DetailNormalScaleProperty);
                editor.DrawSlider(DetailInputsStyles.DetailSmoothnessScale, DetailSmoothnessScaleProperty);
            });
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(DetailMapID))
                CoreUtils.SetKeyword(material, "_DETAIL", material.GetTexture(DetailMapID));
        }
    }
}