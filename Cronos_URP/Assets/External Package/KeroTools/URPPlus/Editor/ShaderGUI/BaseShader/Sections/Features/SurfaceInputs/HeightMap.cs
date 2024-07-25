using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class HeightMap : IDrawable
    {
        private const float CentimetersToMeters = 0.01f;
        private const float MinAmplitudeThreshold = 1e-6f;

        private static readonly int HeightMapID = Shader.PropertyToID("_HeightMap");
        private static readonly int DisplacementModeID = Shader.PropertyToID("_DisplacementMode");
        private static readonly int HeightAmplitudeID = Shader.PropertyToID("_HeightAmplitude");
        private static readonly int HeightCenterID = Shader.PropertyToID("_HeightCenter");
        private readonly Material _material;

        protected readonly string[] HeightParametrizationOptions = Enum.GetNames(typeof(HeightParametrization));

        private float _heightAmplitude;
        private float _heightCenter = 1.0f;

        protected MaterialProperty HeightMapProperty;
        protected MaterialProperty HeightMaxProperty;
        protected MaterialProperty HeightMinProperty;
        protected MaterialProperty HeightOffsetProperty;
        protected MaterialProperty HeightParametrizationProperty;
        protected MaterialProperty HeightPoMAmplitudeProperty;
        protected MaterialProperty HeightTessAmplitudeProperty;
        protected MaterialProperty HeightTessCenterProperty;

        public HeightMap(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            HeightMapProperty = GetProperty("_HeightMap");
            HeightParametrizationProperty = GetProperty("_HeightMapParametrization");
            HeightTessCenterProperty = GetProperty("_HeightTessCenter");
            HeightTessAmplitudeProperty = GetProperty("_HeightTessAmplitude");
            HeightMinProperty = GetProperty("_HeightMin");
            HeightMaxProperty = GetProperty("_HeightMax");
            HeightOffsetProperty = GetProperty("_HeightOffset");
            HeightPoMAmplitudeProperty = GetProperty("_HeightPoMAmplitude");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var displacementMode = (DisplacementMode)_material.GetFloat(DisplacementModeID);

            if (displacementMode == DisplacementMode.None)
                return;

            DrawHeightMap(editor);
            DrawHeightParametrization(editor);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(HeightMapID))
                CoreUtils.SetKeyword(material, "_HEIGHTMAP", material.GetTexture(HeightMapID));
        }

        protected virtual void DrawHeightMap(PropertiesEditor editor) => 
            editor.DrawTexture(HeightBlockStyles.HeightMap, HeightMapProperty);

        protected virtual void DrawHeightParametrizationMode(PropertiesEditor editor)
        {
            editor.DrawPopup(HeightBlockStyles.HeightMapParametrization, HeightParametrizationProperty,
                HeightParametrizationOptions);
        }

        private void DrawHeightParametrization(PropertiesEditor editor)
        {
            var displacementMode = (DisplacementMode)_material.GetFloat(DisplacementModeID);

            if (displacementMode == DisplacementMode.PixelDisplacement)
                DrawPPDHeightProperties(editor);
            else
                DrawVertexHeightProperties(editor);
        }

        protected virtual void DrawPPDHeightProperties(PropertiesEditor editor)
        {
            editor.DrawIndented(() =>
            {
                editor.DrawFloat(HeightBlockStyles.HeightMapAmplitude, HeightPoMAmplitudeProperty);
                _material.SetFloat(HeightAmplitudeID, HeightPoMAmplitudeProperty.floatValue * CentimetersToMeters);
                _material.SetFloat(HeightCenterID, 1.0f);
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

            _material.SetFloat(HeightAmplitudeID, _heightAmplitude);
            _material.SetFloat(HeightCenterID, _heightCenter);
        }

        protected virtual void DrawHeightParametrizationProperties(PropertiesEditor editor)
        {
            var selectedParametrization = (HeightParametrization)HeightParametrizationProperty.floatValue;
            switch (selectedParametrization)
            {
                case ShaderGUI.HeightParametrization.Amplitude:
                    DrawAmplitudeMode(editor);
                    break;
                case ShaderGUI.HeightParametrization.MinMax:
                    DrawMinMaxMode(editor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void DrawAmplitudeMode(PropertiesEditor editor)
        {
            editor.DrawFloat(HeightBlockStyles.HeightMapAmplitude, HeightTessAmplitudeProperty);
            editor.DrawSlider(HeightBlockStyles.HeightMapCenter, HeightTessCenterProperty);

            var offset = HeightOffsetProperty.floatValue;
            var amplitude = HeightTessAmplitudeProperty.floatValue;
            var center = HeightTessCenterProperty.floatValue;
            _heightAmplitude = amplitude * CentimetersToMeters;
            _heightCenter = -offset / Mathf.Max(MinAmplitudeThreshold, amplitude) + center;
        }

        protected virtual void DrawMinMaxMode(PropertiesEditor editor)
        {
            editor.DrawFloat(HeightBlockStyles.HeightMapMin, HeightMinProperty);
            editor.DrawFloat(HeightBlockStyles.HeightMapMax, HeightMaxProperty);

            var offset = HeightOffsetProperty.floatValue;
            var minHeight = HeightMinProperty.floatValue;
            var maxHeight = HeightMaxProperty.floatValue - minHeight;
            _heightAmplitude = maxHeight * CentimetersToMeters;
            _heightCenter = -(minHeight + offset) / Mathf.Max(MinAmplitudeThreshold, maxHeight);
        }

        protected virtual void DrawHeightOffset(PropertiesEditor editor) => 
            editor.DrawFloat(HeightBlockStyles.HeightMapOffset, HeightOffsetProperty);
    }
}