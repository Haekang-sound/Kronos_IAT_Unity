using System;
using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerHeightMap : LayerFeature
    {
        private readonly Material _material;
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string HeightMap = "_HeightMap";
        private const string HeightParametrization = "_HeightMapParametrization";
        private const string HeightTessCenter = "_HeightTessCenter";
        private const string HeightTessAmplitude = "_HeightTessAmplitude";
        private const string HeightMin = "_HeightMin";
        private const string HeightMax = "_HeightMax";
        private const string HeightOffset = "_HeightOffset";
        private const string HeightPoMAmplitude = "_HeightPoMAmplitude";

        protected MaterialProperty[] HeightMapProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightParametrizationProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightTessCenterProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightTessAmplitudeProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightMinProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightMaxProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightOffsetProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] HeightPoMAmplitudeProperties = new MaterialProperty[MaxLayersCount];

        protected readonly string[] HeightParametrizationOptions = Enum.GetNames(typeof(HeightParametrization));

        private float _heightAmplitude;
        private float _heightCenter = 1.0f;

        private const float CentimetersToMeters = 0.01f;
        private const float MinAmplitudeThreshold = 1e-6f;

        private static readonly int DisplacementModeID = Shader.PropertyToID("_DisplacementMode");
        private static readonly int[] HeightAmplitudeIDs = 
        {
            Shader.PropertyToID("_HeightAmplitude"),
            Shader.PropertyToID("_HeightAmplitude1"),
            Shader.PropertyToID("_HeightAmplitude2"),
            Shader.PropertyToID("_HeightAmplitude3"),
        };

        private static readonly int[] HeightCenterIDs =
        {
            Shader.PropertyToID("_HeightCenter"),
            Shader.PropertyToID("_HeightCenter1"),
            Shader.PropertyToID("_HeightCenter2"),
            Shader.PropertyToID("_HeightCenter3"),
        };

        public LayerHeightMap(Material material, uint layersCount, int layerIndex)
        {
            _material = material;
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            HeightMapProperties = GetLayerProperty(HeightMap);
            HeightParametrizationProperties = GetLayerProperty(HeightParametrization);
            HeightTessCenterProperties = GetLayerProperty(HeightTessCenter);
            HeightTessAmplitudeProperties = GetLayerProperty(HeightTessAmplitude);
            HeightMinProperties = GetLayerProperty(HeightMin);
            HeightMaxProperties = GetLayerProperty(HeightMax);
            HeightOffsetProperties = GetLayerProperty(HeightOffset);
            HeightPoMAmplitudeProperties = GetLayerProperty(HeightPoMAmplitude);
            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor)
        {
            var displacementMode = (DisplacementMode)_material.GetFloat(DisplacementModeID);

            if (displacementMode == DisplacementMode.None)
                return;

            DrawHeightMap(editor);
            DrawHeightParametrization(editor);
        }

        protected virtual void DrawHeightMap(PropertiesEditor editor) => 
            editor.DrawTexture(HeightBlockStyles.HeightMap, HeightMapProperties[_layerIndex]);

        protected virtual void DrawHeightParametrizationMode(PropertiesEditor editor) =>
            editor.DrawPopup(HeightBlockStyles.HeightMapParametrization, HeightParametrizationProperties[_layerIndex], HeightParametrizationOptions);

        private void DrawHeightParametrization(PropertiesEditor editor)
        {
            var displacementMode = (DisplacementMode)_material.GetFloat(DisplacementModeID);
            switch (displacementMode)
            {
                case DisplacementMode.PixelDisplacement:
                    DrawPPDHeightProperties(editor);
                    break;
                case DisplacementMode.VertexDisplacement:
                    DrawVertexHeightProperties(editor);
                    break;
            }
        }

        protected virtual void DrawPPDHeightProperties(PropertiesEditor editor)
        {
            editor.DrawIndented(() =>
            {
                editor.DrawFloat(HeightBlockStyles.HeightMapAmplitude, HeightPoMAmplitudeProperties[_layerIndex]);
                _material.SetFloat(HeightAmplitudeIDs[_layerIndex], HeightPoMAmplitudeProperties[_layerIndex].floatValue * CentimetersToMeters);
                _material.SetFloat(HeightCenterIDs[_layerIndex], 1.0f);
            });
        }

        protected virtual void DrawVertexHeightProperties(PropertiesEditor editor)
        {
            editor.DrawIndented(() =>
            {
                DrawHeightParametrizationMode(editor);
                DrawHeightParametrizationProperties(editor);
                DrawHeightOffset(editor);
            });

            _material.SetFloat(HeightAmplitudeIDs[_layerIndex], _heightAmplitude);
            _material.SetFloat(HeightCenterIDs[_layerIndex], _heightCenter);
        }

        protected virtual void DrawHeightParametrizationProperties(PropertiesEditor editor)
        {
            var selectedParametrization = (HeightParametrization)HeightParametrizationProperties[_layerIndex].floatValue;
            switch (selectedParametrization)
            {
                case ShaderGUI.HeightParametrization.Amplitude:
                    DrawAmplitudeMode(editor);
                    break;
                case ShaderGUI.HeightParametrization.MinMax:
                    DrawMinMaxMode(editor);
                    break;
            }
        }

        protected virtual void DrawAmplitudeMode(PropertiesEditor editor)
        {
            editor.DrawFloat(HeightBlockStyles.HeightMapAmplitude, HeightTessAmplitudeProperties[_layerIndex]);
            editor.DrawSlider(HeightBlockStyles.HeightMapCenter, HeightTessCenterProperties[_layerIndex]);

            var offset = HeightOffsetProperties[_layerIndex].floatValue;
            var amplitude = HeightTessAmplitudeProperties[_layerIndex].floatValue;
            var center = HeightTessCenterProperties[_layerIndex].floatValue;
            _heightAmplitude = amplitude * CentimetersToMeters;
            _heightCenter = -offset / Mathf.Max(MinAmplitudeThreshold, amplitude) + center;
        }

        protected virtual void DrawMinMaxMode(PropertiesEditor editor)
        {
            editor.DrawFloat(HeightBlockStyles.HeightMapMin, HeightMinProperties[_layerIndex]);
            editor.DrawFloat(HeightBlockStyles.HeightMapMax, HeightMaxProperties[_layerIndex]);

            var offset = HeightOffsetProperties[_layerIndex].floatValue;
            var minHeight = HeightMinProperties[_layerIndex].floatValue;
            var maxHeight = HeightMaxProperties[_layerIndex].floatValue - minHeight;
            _heightAmplitude = maxHeight * CentimetersToMeters;
            _heightCenter = -(minHeight + offset) / Mathf.Max(MinAmplitudeThreshold, maxHeight);
        }

        protected virtual void DrawHeightOffset(PropertiesEditor editor)
        {
            editor.DrawFloat(HeightBlockStyles.HeightMapOffset, HeightOffsetProperties[_layerIndex]);
        }

        public override void SetKeywords(Material material)
        {
            var layerHeightMapID = LayerUtils.LayerProperty(HeightMap, _layerIndex);
            if (material.HasProperty(layerHeightMapID))
            {
                var hasLayerHeightMap = material.GetTexture(layerHeightMapID);
                CoreUtils.SetKeyword(material, LayerUtils.LayerProperty("_HEIGHTMAP", _layerIndex), hasLayerHeightMap);
            }
        }
    }
}