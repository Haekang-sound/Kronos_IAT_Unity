using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class TransparencyInputs : Section
    {
        private readonly Material _material;

        protected MaterialProperty RefractionEnableProperty;
        protected MaterialProperty IorProperty;
        protected MaterialProperty TransmittanceColorProperty;
        protected MaterialProperty TransmittanceColorMapProperty;
        protected MaterialProperty ChromaticAberrationEnableProperty;
        protected MaterialProperty ChromaticAberrationProperty;
        protected MaterialProperty RefractionShadowAttenuationProperty;

        private static readonly int SurfaceTypeID = Shader.PropertyToID("_Surface");
        private static readonly int RefractionEnableID = Shader.PropertyToID("_RefractionEnable");
        private static readonly int ChromaticAberrationEnableID = Shader.PropertyToID("_ChromaticAberrationEnable");

        public TransparencyInputs(Material material)
        {
            _material = material;

            Label = TransparencyStyles.Label;
            Expandable = Expandable.TransparencyInputs;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            var surfaceType = (SurfaceTypeMode)_material.GetFloat(SurfaceTypeID);
            IsRendered = surfaceType == SurfaceTypeMode.Transparent;

            RefractionEnableProperty = GetProperty("_RefractionEnable");
            IorProperty = GetProperty("_Ior");
            TransmittanceColorProperty = GetProperty("_TransmittanceColor");
            TransmittanceColorMapProperty = GetProperty("_TransmittanceColorMap");
            ChromaticAberrationEnableProperty = GetProperty("_ChromaticAberrationEnable");
            ChromaticAberrationProperty = GetProperty("_ChromaticAberration");
            RefractionShadowAttenuationProperty = GetProperty("_RefractionShadowAttenuation");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            var refractionToggle = DrawRefractionToggle(editor);

            if (!refractionToggle)
                return;

            editor.DrawIndented(() =>
            {
                DrawIorSlider(editor);
                DrawTransmittanceMap(editor);
                DrawChromaticAberration(editor);
                DrawRefractionShadowAttenuationSlider(editor);
            });
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(RefractionEnableID))
                CoreUtils.SetKeyword(material, "_REFRACTION",
                    material.GetFloat(RefractionEnableID) > 0.5f && IsRendered);

            if (material.HasProperty(ChromaticAberrationEnableID))
                CoreUtils.SetKeyword(material, "_CHROMATIC_ABERRATION",
                    material.GetFloat(ChromaticAberrationEnableID) > 0.5f);
        }

        private bool DrawRefractionToggle(PropertiesEditor editor)
        {
            var options = Enum.GetNames(typeof(BoolMode));

            return editor.DrawBoolPopup(TransparencyStyles.EnableRefraction, RefractionEnableProperty, options);
        }

        private void DrawIorSlider(PropertiesEditor editor)
        {
            editor.DrawSlider(TransparencyStyles.RefractionIor, IorProperty, -1.0f, 1.0f);
        }
        
        private void DrawTransmittanceMap(PropertiesEditor editor)
        {
            editor.DrawTexture(TransparencyStyles.TransmittanceColor, TransmittanceColorMapProperty, TransmittanceColorProperty);
        }

        private void DrawChromaticAberration(PropertiesEditor editor)
        {
            var chromaticAberrationToggle = DrawChromaticAberrationToggle(editor);

            if (!chromaticAberrationToggle)
                return;

            editor.DrawIndented(() => DrawAberrationSlider(editor));
        }

        private bool DrawChromaticAberrationToggle(PropertiesEditor editor) => 
            editor.DrawToggle(TransparencyStyles.EnableChromaticAberration, ChromaticAberrationEnableProperty);

        private void DrawAberrationSlider(PropertiesEditor editor) => 
            editor.DrawSlider(TransparencyStyles.ChromaticAberration, ChromaticAberrationProperty);
        
        private void DrawRefractionShadowAttenuationSlider(PropertiesEditor editor) => 
            editor.DrawSlider(TransparencyStyles.RefractionShadowAttenuation, RefractionShadowAttenuationProperty);
    }
}