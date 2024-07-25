using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerTextureOffset : LayerFeature
    {
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string BaseMap = "_BaseMap";

        protected MaterialProperty[] BaseMapProperties = new MaterialProperty[MaxLayersCount];

        public LayerTextureOffset(uint layersCount, int layerIndex)
        {
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties) => 
            BaseMapProperties = LayerUtils.FindLayerProperty(BaseMap, properties, _layersCount);

        public override void Draw(PropertiesEditor editor) => 
            editor.DrawTextureScaleOffset(BaseMapProperties[_layerIndex]);

        public override void SetKeywords(Material material)
        {
        }
    }
}