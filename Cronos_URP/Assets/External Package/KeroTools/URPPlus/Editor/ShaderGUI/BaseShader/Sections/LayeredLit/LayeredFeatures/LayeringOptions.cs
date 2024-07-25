using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayeringOptions : LayerFeature
    {
        private readonly Material _material;
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string OpacityAsDensity = "_OpacityAsDensity";
        private const string InheritBaseColor = "_InheritBaseColor";
        private const string InheritBaseNormal = "_InheritBaseNormal";
        private const string InheritBaseHeight = "_InheritBaseHeight";

        protected MaterialProperty LayerInfluenceMaskMap;

        protected MaterialProperty[] OpacityAsDensityProperties;
        protected MaterialProperty[] InheritBaseColorProperties = new MaterialProperty[MaxLayersCount - 1];
        protected MaterialProperty[] InheritBaseNormalProperties = new MaterialProperty[MaxLayersCount - 1];
        protected MaterialProperty[] InheritBaseHeightProperties = new MaterialProperty[MaxLayersCount - 1];

        private static readonly int UseMainLayerInfluenceID = Shader.PropertyToID("_UseMainLayerInfluence");
        private static readonly int LayerInfluenceMaskMapID = Shader.PropertyToID("_LayerInfluenceMaskMap");

        public LayeringOptions(Material material, uint layersCount, int layerIndex)
        {
            _material = material;
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            LayerInfluenceMaskMap = PropertyFinder.FindOptionalProperty("_LayerInfluenceMaskMap", properties);

            OpacityAsDensityProperties = GetLayerProperty(OpacityAsDensity);

            for (var i = 1; i < MaxLayersCount; ++i)
            {
                InheritBaseColorProperties[i - 1] =
                    PropertyFinder.FindOptionalProperty($"{InheritBaseColor}{i}", properties);
                InheritBaseNormalProperties[i - 1] =
                    PropertyFinder.FindOptionalProperty($"{InheritBaseNormal}{i}", properties);
                InheritBaseHeightProperties[i - 1] =
                    PropertyFinder.FindOptionalProperty($"{InheritBaseHeight}{i}", properties);
            }

            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor)
        {
            if (_layerIndex == 0)
                DrawLayerInfluenceMaskMap(editor);
            else
                DrawInfluenceProperties(editor);
        }

        protected virtual void DrawLayerInfluenceMaskMap(PropertiesEditor editor)
        {
            editor.DrawTexture(LayeredStyles.LayerInfluenceMaskMap, LayerInfluenceMaskMap);
            EditorGUILayout.Space();
        }

        protected virtual void DrawInfluenceProperties(PropertiesEditor editor)
        {
            editor.DrawToggle(LayeredStyles.OpacityAsDensity, OpacityAsDensityProperties[_layerIndex]);

            var mainInfluenceState = _material.GetFloat(UseMainLayerInfluenceID) > 0.5f;
            if (!mainInfluenceState) 
                return;
            
            var inheritIndex = _layerIndex - 1;
            editor.DrawSlider(LayeredStyles.InheritBaseColor, InheritBaseColorProperties[inheritIndex]);
            editor.DrawSlider(LayeredStyles.InheritBaseNormal, InheritBaseNormalProperties[inheritIndex]);
            editor.DrawSlider(LayeredStyles.InheritBaseHeight, InheritBaseHeightProperties[inheritIndex]);
            EditorGUILayout.Space();
        }

        public override void SetKeywords(Material material)
        {
            if (!material.HasProperty(UseMainLayerInfluenceID) ||
                !material.HasProperty(LayerInfluenceMaskMapID)) 
                return;
            
            var hasInfluenceMaskMap = material.GetTexture(LayerInfluenceMaskMapID);
            var usedMainLayerInfluence = material.GetFloat(UseMainLayerInfluenceID) > 0.5f;
            var influenceMaskMapState = hasInfluenceMaskMap && usedMainLayerInfluence;

            CoreUtils.SetKeyword(material, "_INFLUENCEMASK_MAP", influenceMaskMapState);
        }
    }
}