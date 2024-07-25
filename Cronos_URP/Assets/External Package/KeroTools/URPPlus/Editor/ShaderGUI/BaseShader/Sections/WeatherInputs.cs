using System;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class WeatherInputs : Section
    {
        private readonly IDrawable _weatherProfile;
        protected MaterialProperty WeatherEnableProperty;
        protected MaterialProperty RainModeProperty;

        protected MaterialProperty PuddlesNormalScaleProperty;
        protected MaterialProperty RainNormalScaleProperty;

        protected MaterialProperty RainDistortionProperty;
        protected MaterialProperty RainDistortionScaleProperty;
        protected MaterialProperty RainDistortionSizeProperty;

        protected MaterialProperty RainMaskProperty;
        protected MaterialProperty RainWetnessFactorProperty;

        private static readonly int WeatherEnableID = Shader.PropertyToID("_WeatherEnable");
        private static readonly int RainModeID = Shader.PropertyToID("_RainMode");
        private static readonly int RainNormalMapID = Shader.PropertyToID("_PuddlesNormal");
        protected readonly string[] RainModeOptions = Enum.GetNames(typeof(RainMode));

        public WeatherInputs(Material material)
        {
            Label = new GUIContent("Weather Inputs");
            Expandable = Expandable.WeatherInputs;
            _weatherProfile = new WeatherProfile(material);
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            WeatherEnableProperty = GetProperty("_WeatherEnable");
            RainModeProperty = GetProperty("_RainMode");
            
            _weatherProfile.FindProperties(properties);

            PuddlesNormalScaleProperty = GetProperty("_PuddlesNormalScale");
            RainNormalScaleProperty = GetProperty("_RainNormalScale");

            RainDistortionProperty = GetProperty("_RainDistortionMap");
            RainDistortionScaleProperty = GetProperty("_RainDistortionScale");
            RainDistortionSizeProperty = GetProperty("_RainDistortionSize");
            RainMaskProperty = GetProperty("_RainMaskMap");
            RainWetnessFactorProperty = GetProperty("_RainWetnessFactor");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public override void Draw(PropertiesEditor editor)
        {
            var drawSettings = editor.DrawToggle(WeatherInputsStyles.WeatherEnable, WeatherEnableProperty);

            if (!drawSettings)
                return;

            editor.DrawBoolPopup(WeatherInputsStyles.RainMode, RainModeProperty, RainModeOptions);
            
            _weatherProfile.Draw(editor);

            editor.DrawSlider(WeatherInputsStyles.PuddlesNormalIntensity, PuddlesNormalScaleProperty, 0.0f, 8.0f);
            editor.DrawSlider(WeatherInputsStyles.RainNormalIntensity, RainNormalScaleProperty, 0.0f, 8.0f);

            editor.DrawTexture(WeatherInputsStyles.RainDistortion, RainDistortionProperty, RainDistortionScaleProperty);
            editor.DrawMinFloat(WeatherInputsStyles.RainDistortionSize, RainDistortionSizeProperty, 0.001f);
            editor.DrawTexture(WeatherInputsStyles.RainMask, RainMaskProperty);
            editor.DrawSlider(WeatherInputsStyles.RainWetnessFactor, RainWetnessFactorProperty);
        }

        public override void SetKeywords(Material material)
        {
            if (material.HasProperty(WeatherEnableID))
                CoreUtils.SetKeyword(material, "_WEATHER_ON", WeatherEnableProperty.floatValue > 0.5f);

            if (material.HasProperty(RainModeID))
                CoreUtils.SetKeyword(material, "_RAIN_TRIPLANAR", (RainMode)RainModeProperty.floatValue == ShaderGUI.RainMode.Triplanar);

            if (material.HasProperty(RainNormalMapID))
                CoreUtils.SetKeyword(material, "_RAIN_NORMALMAP", material.GetTexture(RainNormalMapID));
        }
    }
}