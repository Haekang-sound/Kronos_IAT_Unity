using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class TessellationOptions : Section
    {
        protected MaterialProperty TessellationModeProperty;
        protected MaterialProperty PhongTessellationEnableProperty;
        protected MaterialProperty TessellationShapeFactorProperty;
        protected MaterialProperty TessellationFactorProperty;
        protected MaterialProperty TessellationEdgeLengthProperty;
        protected MaterialProperty TessellationFactorMinDistanceProperty;
        protected MaterialProperty TessellationFactorMaxDistanceProperty;
        protected MaterialProperty TessellationBackFaceCullEpsilonProperty;

        private static readonly int TessellationModeID = Shader.PropertyToID("_TessellationMode");
        private static readonly int PhongTessellationModeID = Shader.PropertyToID("_PhongTessellationMode");

        public TessellationOptions()
        {
            Label = TessellationStyles.Label;
            Expandable = Expandable.TessellationOptions;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            TessellationModeProperty = GetProperty("_TessellationMode");
            PhongTessellationEnableProperty = GetProperty("_PhongTessellationMode");
            TessellationShapeFactorProperty = GetProperty("_TessellationShapeFactor");
            TessellationFactorProperty = GetProperty("_TessellationFactor");
            TessellationEdgeLengthProperty = GetProperty("_TessellationEdgeLength");
            TessellationFactorMinDistanceProperty = GetProperty("_TessellationFactorMinDistance");
            TessellationFactorMaxDistanceProperty = GetProperty("_TessellationFactorMaxDistance");
            TessellationBackFaceCullEpsilonProperty = GetProperty("_TessellationBackFaceCullEpsilon");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            DrawTessellationMode(editor);
            DrawPhongTessellationToggle(editor);
            DrawTessellationFactor(editor);
            DrawModeBasedProperties(editor);
            DrawCullingSlider(editor);
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(TessellationModeID))
            {
                var tessellationModeEnum = (TessellationMode)material.GetFloat(TessellationModeID);
                switch (tessellationModeEnum)
                {
                    case ShaderGUI.TessellationMode.None:
                        material.DisableKeyword("_TESSELLATION_DISTANCE");
                        material.DisableKeyword("_TESSELLATION_EDGE");
                        break;
                    case ShaderGUI.TessellationMode.EdgeLength:
                        material.DisableKeyword("_TESSELLATION_DISTANCE");
                        material.EnableKeyword("_TESSELLATION_EDGE");
                        break;
                    case ShaderGUI.TessellationMode.Distance:
                        material.DisableKeyword("_TESSELLATION_EDGE");
                        material.EnableKeyword("_TESSELLATION_DISTANCE");
                        break;
                }
            }

            if (material.HasProperty(PhongTessellationModeID))
            {
                var phongTessellationState = material.GetFloat(PhongTessellationModeID) > 0.5f;
                CoreUtils.SetKeyword(material, "_TESSELLATION_PHONG", phongTessellationState);
            }
        }

        protected virtual void DrawTessellationMode(PropertiesEditor editor)
        {
            var options = Enum.GetNames(typeof(TessellationMode));
            editor.DrawPopup(TessellationStyles.Mode, TessellationModeProperty, options);
        }

        protected virtual void DrawPhongTessellationToggle(PropertiesEditor editor)
        {
            var phongTessellation = editor.DrawToggle(TessellationStyles.PhongTessellation, PhongTessellationEnableProperty);

            if (!phongTessellation)
                return;

            editor.DrawSlider(TessellationStyles.ShapeFactor, TessellationShapeFactorProperty);
        }

        protected virtual void DrawModeBasedProperties(PropertiesEditor editor)
        {
            var mode = GetTessellationMode();
            DrawTriangleSizeTessellation(mode, editor);
            DrawDistanceTessellation(mode, editor);
        }

        protected virtual void DrawTessellationFactor(PropertiesEditor editor) => 
            editor.DrawSlider(TessellationStyles.Factor, TessellationFactorProperty);

        protected virtual void DrawTriangleSizeTessellation(TessellationMode mode, PropertiesEditor editor)
        {
            if (mode != TessellationMode.EdgeLength)
                return;

            editor.DrawSlider(TessellationStyles.FactorTriangleSize, TessellationEdgeLengthProperty);
        }

        protected virtual void DrawDistanceTessellation(TessellationMode mode, PropertiesEditor editor)
        {
            if (mode != TessellationMode.Distance)
                return;

            editor.DrawFloat(TessellationStyles.FactorMinDistance, TessellationFactorMinDistanceProperty);
            editor.DrawFloat(TessellationStyles.FactorMaxDistance, TessellationFactorMaxDistanceProperty);
        }

        protected virtual void DrawCullingSlider(PropertiesEditor editor) => 
            editor.DrawSlider(TessellationStyles.BackFaceCullEpsilon, TessellationBackFaceCullEpsilonProperty);

        protected virtual TessellationMode GetTessellationMode() => 
            (TessellationMode)TessellationModeProperty.floatValue;
    }
}