using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class SurfaceType : IDrawable
    {
        protected readonly string[] BlendModeOptions = Enum.GetNames(typeof(BlendMode));
        protected readonly string[] DepthTestOptions = Enum.GetNames(typeof(CompareFunction));
        protected readonly string[] SurfaceTypeOptions = Enum.GetNames(typeof(SurfaceTypeMode));
        private readonly Material _material;
        
        protected MaterialProperty SurfaceTypeProperty;
        protected MaterialProperty BlendModeProperty;
        protected MaterialProperty DepthTestProperty;
        protected MaterialProperty DepthWriteProperty;
        protected MaterialProperty PreserveSpecularProperty;
        
        public SurfaceType(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            SurfaceTypeProperty = GetProperty("_Surface");
            BlendModeProperty = GetProperty("_Blend");
            PreserveSpecularProperty = GetProperty("_BlendModePreserveSpecular");

            DepthWriteProperty = GetProperty("_ZWrite");
            DepthTestProperty = GetProperty("_ZTest");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var isTransparent = DrawSurfaceType(editor);

            if (!isTransparent)
                return;

            editor.DrawIndented(() =>
            {
                DrawBlendMode(editor);
                DrawDepthWrite(editor);
                DrawDepthTest(editor);
            });
        }


        public void SetKeywords(Material material)
        {
            SetupMaterialBlendModeInternal(material, out var renderQueue);

            // apply automatic render queue
            if (renderQueue != material.renderQueue)
                material.renderQueue = renderQueue;
        }

        protected virtual bool DrawSurfaceType(PropertiesEditor editor)
        {
            editor.DrawPopup(SurfaceOptionsStyles.SurfaceType, SurfaceTypeProperty, SurfaceTypeOptions);

            return (SurfaceTypeMode)SurfaceTypeProperty.floatValue == SurfaceTypeMode.Transparent;
        }

        protected virtual void DrawBlendMode(PropertiesEditor editor)
        {
            editor.DrawPopup(SurfaceOptionsStyles.BlendingMode, BlendModeProperty, BlendModeOptions);

            DrawPreserveSpecular(editor);
        }

        protected virtual void DrawPreserveSpecular(PropertiesEditor editor)
        {
            if (!_material.HasProperty("_BlendModePreserveSpecular"))
                return;

            var blendMode = (BlendMode)BlendModeProperty.floatValue;
            var isDisabled = blendMode is BlendMode.Multiply or BlendMode.Premultiply;

            if (isDisabled)
                return;

            editor.DrawIndented(() => DrawPreserveSpecularToggle(editor));
        }

        protected virtual void DrawPreserveSpecularToggle(PropertiesEditor editor) => 
            editor.DrawToggle(SurfaceOptionsStyles.PreserveSpecular, PreserveSpecularProperty);

        protected virtual void DrawDepthWrite(PropertiesEditor editor) => 
            editor.DrawToggle(SurfaceOptionsStyles.ZWriteEnable, DepthWriteProperty);

        protected virtual void DrawDepthTest(PropertiesEditor editor) => 
            editor.DrawPopup(SurfaceOptionsStyles.TransparentZTest, DepthTestProperty, DepthTestOptions);

        private void SetMaterialSrcDstBlendProperties(Material material,
            UnityEngine.Rendering.BlendMode srcBlend, UnityEngine.Rendering.BlendMode dstBlend)
        {
            if (material.HasProperty("_SrcBlend"))
                material.SetFloat("_SrcBlend", (float)srcBlend);

            if (material.HasProperty("_DstBlend"))
                material.SetFloat("_DstBlend", (float)dstBlend);

            if (material.HasProperty("_SrcBlendAlpha"))
                material.SetFloat("_SrcBlendAlpha", (float)srcBlend);

            if (material.HasProperty("_DstBlendAlpha"))
                material.SetFloat("_DstBlendAlpha", (float)dstBlend);
        }

        private void SetMaterialSrcDstBlendProperties(Material material,
            UnityEngine.Rendering.BlendMode srcBlendRGB, UnityEngine.Rendering.BlendMode dstBlendRGB,
            UnityEngine.Rendering.BlendMode srcBlendAlpha, UnityEngine.Rendering.BlendMode dstBlendAlpha)
        {
            if (material.HasProperty("_SrcBlend"))
                material.SetFloat("_SrcBlend", (float)srcBlendRGB);

            if (material.HasProperty("_DstBlend"))
                material.SetFloat("_DstBlend", (float)dstBlendRGB);

            if (material.HasProperty("_SrcBlendAlpha"))
                material.SetFloat("_SrcBlendAlpha", (float)srcBlendAlpha);

            if (material.HasProperty("_DstBlendAlpha"))
                material.SetFloat("_DstBlendAlpha", (float)dstBlendAlpha);
        }

        private static void SetMaterialZWriteProperty(Material material, bool zwriteEnabled)
        {
            if (material.HasProperty("_ZWrite"))
                material.SetFloat("_ZWrite", zwriteEnabled ? 1.0f : 0.0f);
        }

        private void SetupMaterialBlendModeInternal(Material material, out int automaticRenderQueue)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            var alphaClip = false;
            if (material.HasProperty("_"))
                alphaClip = material.GetFloat("_AlphaCutoffEnable") >= 0.5;
            CoreUtils.SetKeyword(material, "_ALPHATEST_ON", alphaClip);

            // default is to use the shader render queue
            var renderQueue = material.shader.renderQueue;
            material.SetOverrideTag("RenderType", ""); // clear override tag
            if (material.HasProperty("_Surface"))
            {
                var surfaceType = (SurfaceTypeMode)material.GetFloat("_Surface");
                var zwrite = false;
                CoreUtils.SetKeyword(material, "_SURFACE_TYPE_TRANSPARENT", surfaceType == SurfaceTypeMode.Transparent);
                if (surfaceType == SurfaceTypeMode.Opaque)
                {
                    if (alphaClip)
                    {
                        renderQueue = (int)RenderQueue.AlphaTest;
                        material.SetOverrideTag("RenderType", "TransparentCutout");
                    }
                    else
                    {
                        renderQueue = (int)RenderQueue.Geometry;
                        material.SetOverrideTag("RenderType", "Opaque");
                    }

                    SetMaterialSrcDstBlendProperties(material, UnityEngine.Rendering.BlendMode.One,
                        UnityEngine.Rendering.BlendMode.Zero);
                    zwrite = true;
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                }
                else // SurfaceType Transparent
                {
                    var blendMode = (BlendMode)material.GetFloat("_Blend");

                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.DisableKeyword("_ALPHAMODULATE_ON");

                    var srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                    var dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                    var srcBlendA = UnityEngine.Rendering.BlendMode.One;
                    var dstBlendA = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;

                    // Specific Transparent Mode Settings
                    switch (blendMode)
                    {
                        // srcRGB * srcAlpha + dstRGB * (1 - srcAlpha)
                        // preserve spec:
                        // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                        case BlendMode.Alpha:
                            srcBlendRGB = UnityEngine.Rendering.BlendMode.SrcAlpha;
                            dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                            srcBlendA = UnityEngine.Rendering.BlendMode.One;
                            dstBlendA = dstBlendRGB;
                            break;

                        // srcRGB < srcAlpha, (alpha multiplied in asset)
                        // srcRGB * 1 + dstRGB * (1 - srcAlpha)
                        case BlendMode.Premultiply:
                            srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                            dstBlendRGB = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                            srcBlendA = srcBlendRGB;
                            dstBlendA = dstBlendRGB;
                            break;

                        // srcRGB * srcAlpha + dstRGB * 1, (alpha controls amount of addition)
                        // preserve spec:
                        // srcRGB * (<in shader> ? 1 : srcAlpha) + dstRGB * (1 - srcAlpha)
                        case BlendMode.Additive:
                            srcBlendRGB = UnityEngine.Rendering.BlendMode.SrcAlpha;
                            dstBlendRGB = UnityEngine.Rendering.BlendMode.One;
                            srcBlendA = UnityEngine.Rendering.BlendMode.One;
                            dstBlendA = dstBlendRGB;
                            break;

                        // srcRGB * 0 + dstRGB * srcRGB
                        // in shader alpha controls amount of multiplication, lerp(1, srcRGB, srcAlpha)
                        // Multiply affects color only, keep existing alpha.
                        case BlendMode.Multiply:
                            srcBlendRGB = UnityEngine.Rendering.BlendMode.DstColor;
                            dstBlendRGB = UnityEngine.Rendering.BlendMode.Zero;
                            srcBlendA = UnityEngine.Rendering.BlendMode.Zero;
                            dstBlendA = UnityEngine.Rendering.BlendMode.One;

                            material.EnableKeyword("_ALPHAMODULATE_ON");
                            break;
                    }

                    // Lift alpha multiply from ROP to shader by setting pre-multiplied _SrcBlend mode.
                    // The intent is to do different blending for diffuse and specular in shader.
                    // ref: http://advances.realtimerendering.com/other/2016/naughty_dog/NaughtyDog_TechArt_Final.pdf
                    var preserveSpecular = material.HasProperty("_BlendModePreserveSpecular") &&
                                           material.GetFloat("_BlendModePreserveSpecular") > 0 &&
                                           blendMode != BlendMode.Multiply && blendMode != BlendMode.Premultiply;
                    if (preserveSpecular)
                    {
                        srcBlendRGB = UnityEngine.Rendering.BlendMode.One;
                        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    }

                    // When doing off-screen transparency accumulation, we change blend factors as described here: https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
                    var offScreenAccumulateAlpha = false;
                    if (offScreenAccumulateAlpha)
                        srcBlendA = UnityEngine.Rendering.BlendMode.Zero;

                    SetMaterialSrcDstBlendProperties(material, srcBlendRGB, dstBlendRGB, // RGB
                        srcBlendA, dstBlendA); // Alpha

                    // General Transparent Material Settings
                    material.SetOverrideTag("RenderType", "Transparent");
                    zwrite = material.GetFloat("_ZWrite") > 0.5f;
                    material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    renderQueue = (int)RenderQueue.Transparent;
                }

                SetMaterialZWriteProperty(material, zwrite);
                material.SetShaderPassEnabled("DepthOnly", zwrite);
            }
            else
            {
                // no surface type property -- must be hard-coded by the shadergraph,
                // so ensure the pass is enabled at the material level
                material.SetShaderPassEnabled("DepthOnly", true);
            }

            // must always apply queue offset, even if not set to material control
            if (material.HasProperty("_QueueOffset"))
                renderQueue += (int)material.GetFloat("_QueueOffset");

            automaticRenderQueue = renderQueue;
        }
    }
}