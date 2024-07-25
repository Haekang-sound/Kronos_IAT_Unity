using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerBaseMap : LayerFeature
    {
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string BaseColor = "_BaseColor";
        private const string BaseMap = "_BaseMap";
        private const string AlphaRemapMin = "_AlphaRemapMin";
        private const string AlphaRemapMax = "_AlphaRemapMax";
        private const string InvTilingScale = "_InvTilingScale";

        private static readonly int ColorID = Shader.PropertyToID("_Color");
        private static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
        private static readonly int BaseMapID = Shader.PropertyToID("_BaseMap");

        protected MaterialProperty[] BaseColorProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] BaseMapProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] AlphaMinProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] AlphaMaxProperties = new MaterialProperty[MaxLayersCount];
        
        protected MaterialProperty[] InvTilingScaleProperties = new MaterialProperty[MaxLayersCount];

        public LayerBaseMap(uint layersCount, int layerIndex)
        {
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            BaseColorProperties = GetLayerProperty(BaseColor);
            BaseMapProperties = GetLayerProperty(BaseMap);
            AlphaMinProperties = GetLayerProperty(AlphaRemapMin);
            AlphaMaxProperties = GetLayerProperty(AlphaRemapMax);
            InvTilingScaleProperties = GetLayerProperty(InvTilingScale);
            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(SurfaceInputsStyles.BaseMap, BaseMapProperties[_layerIndex], BaseColorProperties[_layerIndex]);

            if (BaseMapProperties[_layerIndex].textureValue is not null)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.AlphaRemapping, AlphaMinProperties[_layerIndex],
                    AlphaMaxProperties[_layerIndex]);
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(MainTexID))
            {
                material.SetTexture(MainTexID, material.GetTexture(BaseMapID));
                material.SetTextureScale(MainTexID, material.GetTextureScale(BaseMapID));
                material.SetTextureOffset(MainTexID, material.GetTextureOffset(BaseMapID));
            }

            if (material.HasProperty(ColorID))
                material.SetColor(ColorID, material.GetColor(BaseColorID));
            
            InvTilingScaleProperties[_layerIndex].floatValue = 2.0f / 
                                                               (Mathf.Abs(BaseMapProperties[_layerIndex].textureScaleAndOffset.x) 
                                                                + Mathf.Abs(BaseMapProperties[_layerIndex].textureScaleAndOffset.y));
        }
    }
}