using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerBentNormal : LayerFeature
    {
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string BentNormalMap = "_BentNormalMap";

        protected MaterialProperty[] BentNormalMapProperties = new MaterialProperty[MaxLayersCount];

        public LayerBentNormal(uint layersCount, int layerIndex)
        {
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties) => 
            BentNormalMapProperties = LayerUtils.FindLayerProperty(BentNormalMap, properties, _layersCount);

        public override void Draw(PropertiesEditor editor) => 
            editor.DrawNormalTexture(SurfaceInputsStyles.BentNormalMap, BentNormalMapProperties[_layerIndex]);

        public override void SetKeywords(Material material)
        {
            var layerBentNormalMapID = LayerUtils.LayerProperty(BentNormalMap, _layerIndex);
            if (!material.HasProperty(layerBentNormalMapID)) 
                return;
            
            var hasLayerBentNormalMap = material.GetTexture(layerBentNormalMapID);
            CoreUtils.SetKeyword(material, LayerUtils.LayerProperty("_BENTNORMALMAP", _layerIndex),
                hasLayerBentNormalMap);
        }
    }
}