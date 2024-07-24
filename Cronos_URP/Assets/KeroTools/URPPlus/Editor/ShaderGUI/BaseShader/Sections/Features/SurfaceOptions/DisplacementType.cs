using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class DisplacementType : IDrawable
    {
        private const string VertexDisplacementKeyword = "_VERTEX_DISPLACEMENT";
        private const string PixelDisplacementKeyword = "_PIXEL_DISPLACEMENT";
        private const string TessellationDisplacementKeyword = "_TESSELLATION_DISPLACEMENT";
        private const string VertexDisplacementLockObjectScaleKeyword = "_VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE";
        private const string PixelDisplacementLockObjectScaleKeyword = "_PIXEL_DISPLACEMENT_LOCK_OBJECT_SCALE";
        private const string LockTilingScaleKeyword  = "_DISPLACEMENT_LOCK_TILING_SCALE";
        private const string DepthOffsetKeyword = "_DEPTHOFFSET_ON";

        private static readonly int DisplacementModeID = Shader.PropertyToID("_DisplacementMode");
        private static readonly int DisplacementLockObjectScaleID = Shader.PropertyToID("_DisplacementLockObjectScale");
        private static readonly int DisplacementLockTilingScaleID = Shader.PropertyToID("_DisplacementLockTilingScale");
        private static readonly int DepthOffsetEnableID = Shader.PropertyToID("_DepthOffsetEnable");

        protected readonly string[] DisplacementModeOptions = Enum.GetNames(typeof(DisplacementMode));
        protected readonly string[] TessDisplacementModeOptions = Enum.GetNames(typeof(TessDisplacementMode));
        
        private readonly Material _material;
        
        protected MaterialProperty DisplacementModeProperty;
        protected MaterialProperty InvPrimScaleProperty;
        protected MaterialProperty LockWithObjectScaleProperty;
        protected MaterialProperty LockTilingScaleProperty;
        protected MaterialProperty PPDLodThresholdProperty;
        protected MaterialProperty PPDMaxSamplesProperty;
        protected MaterialProperty PPDMinSamplesProperty;
        protected MaterialProperty PPDPrimitiveLengthProperty;
        protected MaterialProperty PPDPrimitiveWidthProperty;
        protected MaterialProperty DepthOffsetProperty;

        public DisplacementType(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            DisplacementModeProperty = GetProperty("_DisplacementMode");
            LockWithObjectScaleProperty = GetProperty("_DisplacementLockObjectScale");
            LockTilingScaleProperty = GetProperty("_DisplacementLockTilingScale");

            PPDMinSamplesProperty = GetProperty("_PPDMinSamples");
            PPDMaxSamplesProperty = GetProperty("_PPDMaxSamples");
            PPDLodThresholdProperty = GetProperty("_PPDLodThreshold");
            PPDPrimitiveLengthProperty = GetProperty("_PPDPrimitiveLength");
            PPDPrimitiveWidthProperty = GetProperty("_PPDPrimitiveWidth");
            InvPrimScaleProperty = GetProperty("_InvPrimScale");
            DepthOffsetProperty = GetProperty("_DepthOffsetEnable");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var mode = GetDisplacementMode();

            DrawDisplacementMode(editor);
            DrawLockWithObjectScale(editor, mode);
            DrawLockWithTilingRate(editor, mode);
            DrawPPDProperties(editor, mode);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(DisplacementModeID))
            {
                var displacementModeValue = material.GetFloat(DisplacementModeID);
                var hasTessellationMode = _material.HasProperty("_TessellationMode");

                if (hasTessellationMode)
                    SetTessellationDisplacementKeywords(material, displacementModeValue);
                else
                    SetDisplacementKeywords(material, displacementModeValue);
            }

            SetLockWithObjectScaleKeywords(material);
            SetDepthOffsetKeyword(material);
        }

        private void DrawDisplacementMode(PropertiesEditor editor)
        {
            var hasTessellationMode = _material.HasProperty("_TessellationMode");
            var options = hasTessellationMode ? TessDisplacementModeOptions : DisplacementModeOptions;

            editor.DrawPopup(SurfaceOptionsStyles.DisplacementMode, DisplacementModeProperty, options);
        }

        private void DrawLockWithObjectScale(PropertiesEditor editor, DisplacementMode mode)
        {
            if (mode == DisplacementMode.None)
                return;

            editor.DrawIndented(() =>
            {
                editor.DrawToggle(SurfaceOptionsStyles.LockWithObjectScale, LockWithObjectScaleProperty);
            });
        }

        private void DrawLockWithTilingRate(PropertiesEditor editor, DisplacementMode mode)
        {
            if (mode == DisplacementMode.None)
                return;
            
            editor.DrawIndented(() =>
            {
                editor.DrawToggle(SurfaceOptionsStyles.LockWithTilingRate, LockTilingScaleProperty);
            });
        }

        private void DrawPPDProperties(PropertiesEditor editor, DisplacementMode mode)
        {
            if (mode != DisplacementMode.PixelDisplacement)
                return;

            EditorGUILayout.Space();
            editor.DrawIndented(() =>
            {
                editor.DrawIntSlider(PPDStyles.PpdMinSamples, PPDMinSamplesProperty);
                editor.DrawIntSlider(PPDStyles.PpdMaxSamples, PPDMaxSamplesProperty);
                editor.DrawSlider(PPDStyles.PpdLodThreshold, PPDLodThresholdProperty);
                DrawPrimitiveScale(editor);
                editor.DrawToggle(new GUIContent("DepthOffset"), DepthOffsetProperty);
            });
        }

        private void DrawPrimitiveScale(PropertiesEditor editor)
        {
            editor.DrawMinFloat(PPDStyles.PpdPrimitiveLength, PPDPrimitiveLengthProperty, 0.01f);
            editor.DrawMinFloat(PPDStyles.PpdPrimitiveWidth, PPDPrimitiveWidthProperty, 0.01f);
            InvPrimScaleProperty.vectorValue =
                new Vector4(1.0f / PPDPrimitiveLengthProperty.floatValue, 1.0f / PPDPrimitiveWidthProperty.floatValue);
        }

        private DisplacementMode GetDisplacementMode()
        {
            if (DisplacementModeProperty is null)
                return DisplacementMode.None;

            return (DisplacementMode)DisplacementModeProperty.floatValue;
        }

        protected void SetDisplacementKeywords(Material material, float displacementModeValue)
        {
            var displacementModeEnum = (DisplacementMode)displacementModeValue;
            switch (displacementModeEnum)
            {
                case DisplacementMode.None:
                    DisableAllKeywords(material);
                    break;
                case DisplacementMode.VertexDisplacement:
                    material.DisableKeyword(PixelDisplacementKeyword);
                    material.DisableKeyword(TessellationDisplacementKeyword);
                    material.EnableKeyword(VertexDisplacementKeyword);
                    break;
                case DisplacementMode.PixelDisplacement:
                    material.DisableKeyword(VertexDisplacementKeyword);
                    material.DisableKeyword(TessellationDisplacementKeyword);
                    material.EnableKeyword(PixelDisplacementKeyword);
                    break;
            }
        }

        protected void SetTessellationDisplacementKeywords(Material material, float displacementModeValue)
        {
            var tessDisplacementModeEnum = (TessDisplacementMode)displacementModeValue;
            if (tessDisplacementModeEnum == TessDisplacementMode.Tessellation)
            {
                material.DisableKeyword(VertexDisplacementKeyword);
                material.DisableKeyword(PixelDisplacementKeyword);
                material.EnableKeyword(TessellationDisplacementKeyword);
            }
            else
            {
                DisableAllKeywords(material);
            }
        }

        protected void DisableAllKeywords(Material material)
        {
            material.DisableKeyword(VertexDisplacementKeyword);
            material.DisableKeyword(PixelDisplacementKeyword);
            material.DisableKeyword(TessellationDisplacementKeyword);
        }

        protected void SetLockWithObjectScaleKeywords(Material material)
        {
            if (!material.HasProperty(DisplacementLockObjectScaleID)) 
                return;
            
            var objectScaleLocked = material.GetFloat(DisplacementLockObjectScaleID) > 0.5f;
            
            CoreUtils.SetKeyword(material, VertexDisplacementLockObjectScaleKeyword, objectScaleLocked);
            CoreUtils.SetKeyword(material, PixelDisplacementLockObjectScaleKeyword, objectScaleLocked);
            
            if (!material.HasProperty(DisplacementLockTilingScaleID)) 
                return;
            
            var tillingScaleLocked = material.GetFloat(DisplacementLockTilingScaleID) > 0.5f;

            CoreUtils.SetKeyword(material, LockTilingScaleKeyword, tillingScaleLocked);
        }

        protected void SetDepthOffsetKeyword(Material material)
        {
            if (!material.HasProperty(DepthOffsetEnableID)) 
                return;
            
            var depthOffsetState = material.GetFloat(DepthOffsetEnableID) > 0.5f;
            CoreUtils.SetKeyword(material, DepthOffsetKeyword, depthOffsetState);
        }
    }
}