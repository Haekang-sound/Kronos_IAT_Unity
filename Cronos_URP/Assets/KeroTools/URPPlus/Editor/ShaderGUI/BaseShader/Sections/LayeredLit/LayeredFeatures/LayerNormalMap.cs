using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerNormalMap : LayerFeature
    {
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string NormalMap = "_NormalMap";
        private const string NormalScale = "_NormalScale";

        protected MaterialProperty[] NormalMapProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] NormalScaleProperties = new MaterialProperty[MaxLayersCount];

        public LayerNormalMap(uint layersCount, int layerIndex)
        {
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            NormalMapProperties = GetLayerProperty(NormalMap);
            NormalScaleProperties = GetLayerProperty(NormalScale);
            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor) => 
            editor.DrawNormalTexture(SurfaceInputsStyles.NormalMap, NormalMapProperties[_layerIndex], NormalScaleProperties[_layerIndex]);

        public override void SetKeywords(Material material)
        {
            var layerNormalMapID = LayerUtils.LayerProperty(NormalMap, _layerIndex);
            
            if (!material.HasProperty(layerNormalMapID)) 
                return;
            
            var hasLayerNormalMap = material.GetTexture(layerNormalMapID);
            CoreUtils.SetKeyword(material, LayerUtils.LayerProperty("_NORMALMAP", _layerIndex), hasLayerNormalMap);
        }
    }
}