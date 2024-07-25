using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerDetailMap : LayerFeature
    {
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string DetailMap = "_DetailMap";
        private const string DetailAlbedoScale = "_DetailAlbedoScale";
        private const string DetailNormalScale = "_DetailNormalScale";
        private const string DetailSmoothnessScale = "_DetailSmoothnessScale";

        protected MaterialProperty[] DetailMapProperties;
        protected MaterialProperty[] DetailAlbedoScaleProperties;
        protected MaterialProperty[] DetailNormalScaleProperties;
        protected MaterialProperty[] DetailSmoothnessScaleProperties;

        public LayerDetailMap(uint layersCount, int layerIndex)
        {
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            DetailMapProperties = GetLayerProperty(DetailMap);
            DetailAlbedoScaleProperties = GetLayerProperty(DetailAlbedoScale);
            DetailNormalScaleProperties = GetLayerProperty(DetailNormalScale);
            DetailSmoothnessScaleProperties = GetLayerProperty(DetailSmoothnessScale);
            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor)
        {
            DrawDetailMap(editor);
            DrawDetailMapProperties(editor);
        }

        private void DrawDetailMap(PropertiesEditor editor) => 
            editor.DrawTexture(DetailInputsStyles.DetailMap, DetailMapProperties[_layerIndex]);

        protected virtual void DrawDetailMapProperties(PropertiesEditor editor)
        {
            if (DetailMapProperties[_layerIndex].textureValue is null)
                return;

            editor.DrawIndented(() =>
            {
                editor.DrawSlider(DetailInputsStyles.DetailAlbedoScale, DetailAlbedoScaleProperties[_layerIndex]);
                editor.DrawSlider(DetailInputsStyles.DetailNormalScale, DetailNormalScaleProperties[_layerIndex]);
                editor.DrawSlider(DetailInputsStyles.DetailSmoothnessScale, DetailSmoothnessScaleProperties[_layerIndex]);
            });
            
            editor.DrawTextureScaleOffset(DetailMapProperties[_layerIndex]);
        }

        public override void SetKeywords(Material material)
        {
            var layerDetailMapID = LayerUtils.LayerProperty(DetailMap, _layerIndex);
            
            if (!material.HasProperty(layerDetailMapID)) 
                return;
            
            var hasLayerMaskMap = material.GetTexture(layerDetailMapID);
            CoreUtils.SetKeyword(material, LayerUtils.LayerProperty("_DETAIL_MAP", _layerIndex), hasLayerMaskMap);
        }
    }
}