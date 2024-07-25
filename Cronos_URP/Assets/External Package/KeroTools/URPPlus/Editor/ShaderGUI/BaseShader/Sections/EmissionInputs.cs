using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class EmissionInputs : Section
    {
        private readonly Material _material;

        protected MaterialProperty EmissionColor;
        protected MaterialProperty EmissionMap;
        protected MaterialProperty EmissionScale;
        protected MaterialProperty EmissionWithBase;
        protected MaterialProperty EnableEmissionFresnel;
        protected MaterialProperty EmissionFresnelPower;

        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
        private static readonly int AlbedoAffectEmissiveID = Shader.PropertyToID("_AlbedoAffectEmissive");
        private static readonly int EmissionFresnelID = Shader.PropertyToID("_EnableEmissionFresnel");

        public EmissionInputs(Material material)
        {
            _material = material;

            Label = EmissionInputsStyles.Label;
            Expandable = Expandable.EmissionInputs;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            EmissionColor = GetProperty("_EmissionColor");
            EmissionMap = GetProperty("_EmissionMap");
            EmissionScale = GetProperty("_EmissionScale");
            EmissionWithBase = GetProperty("_AlbedoAffectEmissive");
            EnableEmissionFresnel = GetProperty("_EnableEmissionFresnel");
            EmissionFresnelPower = GetProperty("_EmissionFresnelPower");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            EditorGUI.BeginChangeCheck();
            DrawEmissionMap(editor);

            editor.DrawIndented(() => DrawEmissionScale(editor));

            if (EditorGUI.EndChangeCheck())
                UpdateGlobalIlluminationFlags();

            DrawMultipliedEmission(editor);
            DrawEmissionFresnel(editor);

            editor.DrawEmissionGIMode();
        }

        protected virtual void DrawEmissionMap(PropertiesEditor editor) => 
            editor.DrawTextureWithHDRColor(EmissionInputsStyles.EmissiveMap, EmissionMap, EmissionColor);

        protected virtual void DrawEmissionScale(PropertiesEditor editor) => 
            editor.DrawFloat(EmissionInputsStyles.EmissiveIntensity, EmissionScale);

        protected virtual void DrawEmissionFresnel(PropertiesEditor editor)
        {
            editor.DrawToggle(EmissionInputsStyles.EmissionFresnel, EnableEmissionFresnel);
            editor.DrawIndented(() =>
            {
                editor.DrawMinFloat(EmissionInputsStyles.EmissionFresnelPower, EmissionFresnelPower, 0.01f);
            });
        }

        protected virtual void DrawMultipliedEmission(PropertiesEditor editor)
        {
            editor.DrawIndented(() =>
            {
                editor.DrawToggle(EmissionInputsStyles.AlbedoAffectEmissive, EmissionWithBase);
            });
        }

        protected virtual void UpdateGlobalIlluminationFlags()
        {
            var brightness = EmissionColor.colorValue.maxColorComponent * EmissionScale.floatValue;
            _material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;

            if (brightness <= 0f) _material.globalIlluminationFlags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(EmissionColorID))
            {
                var emissionState = EmissionColor.colorValue != new Color(0, 0, 0);
                CoreUtils.SetKeyword(material, "_EMISSION", emissionState);
            }

            if (material.HasProperty(AlbedoAffectEmissiveID))
            {
                var emissionWithBaseState = EmissionWithBase.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, "_EMISSION_WITH_BASE", emissionWithBaseState);
            }

            if (material.HasProperty(EmissionFresnelID))
            {
                var emissionFresnelState = EnableEmissionFresnel.floatValue > 0.5f;
                CoreUtils.SetKeyword(material, "_EMISSION_FRESNEL", emissionFresnelState);
            }
        }
    }
}