using System;
using System.Collections.Generic;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class RenderFace : IDrawable
    {
        private static readonly int CullModeID = Shader.PropertyToID("_Cull");
        private static readonly int DoubleSidedNormalModeID = Shader.PropertyToID("_DoubleSidedNormalMode");
        private static readonly int DoubleSidedConstantsID = Shader.PropertyToID("_DoubleSidedConstants");

        private static readonly Dictionary<DoubleSidedNormalMode, Vector4> DoubleSidedNormalModeMap = new()
        {
            { DoubleSidedNormalMode.Mirror, new Vector4(1.0f, 1.0f, -1.0f, 0.0f) },
            { DoubleSidedNormalMode.Flip, new Vector4(-1.0f, -1.0f, -1.0f, 0.0f) },
            { DoubleSidedNormalMode.None, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) }
        };
        
        protected readonly string[] DoubleSidedNormalModeOptions = Enum.GetNames(typeof(DoubleSidedNormalMode));
        protected readonly string[] RenderFaceModeOptions = Enum.GetNames(typeof(RenderFaceMode));
        private readonly Material _material;
        
        protected MaterialProperty DoubleSidedNormalProperty;
        protected MaterialProperty RenderFaceProperty;

        public RenderFace(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            RenderFaceProperty = GetProperty("_Cull");
            DoubleSidedNormalProperty = GetProperty("_DoubleSidedNormalMode");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            DrawRenderFace(editor);
            editor.DrawIndented(() => DrawDoubleSidedNormals(editor));
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(CullModeID))
                material.doubleSidedGI = (RenderFaceMode)material.GetFloat(CullModeID) != RenderFaceMode.Front;

            var renderedBothFaces = (RenderFaceMode)material.GetFloat(CullModeID) == RenderFaceMode.Both;
            var doubleSidedNormals = (DoubleSidedNormalMode)material.GetFloat(DoubleSidedNormalModeID) !=
                                     DoubleSidedNormalMode.None;

            CoreUtils.SetKeyword(material, "_DOUBLESIDED_ON", renderedBothFaces && doubleSidedNormals);
        }

        protected virtual void DrawRenderFace(PropertiesEditor editor) => 
            editor.DrawPopup(SurfaceOptionsStyles.Culling, RenderFaceProperty, RenderFaceModeOptions);

        protected virtual void DrawDoubleSidedNormals(PropertiesEditor editor)
        {
            var isRenderBoth = (RenderFaceMode)_material.GetFloat(CullModeID) == RenderFaceMode.Both;
            var propertiesAreNull = RenderFaceProperty is null || DoubleSidedNormalProperty is null;

            if (propertiesAreNull || !isRenderBoth)
                return;

            if (DoubleSidedNormalProperty is null)
                return;

            editor.DrawPopup(SurfaceOptionsStyles.DoubleSidedNormalMode, DoubleSidedNormalProperty,
                DoubleSidedNormalModeOptions);

            SetDoubleSidedConstants(_material);
        }

        protected virtual void SetDoubleSidedConstants(Material material)
        {
            var mode = (DoubleSidedNormalMode)DoubleSidedNormalProperty.floatValue;

            var hasConstants = DoubleSidedNormalModeMap.TryGetValue(mode, out var constants);

            var sidedConstants = hasConstants ? constants : Vector4.one;

            material.SetVector(DoubleSidedConstantsID, sidedConstants);
        }
    }
}