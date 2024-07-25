using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Hair
{
    public class SpecularBlock : IDrawable
    {
        protected MaterialProperty SecondarySpecularMultiplierProperty;
        protected MaterialProperty SecondarySpecularShiftProperty;
        protected MaterialProperty SmoothnessProperty;
        protected MaterialProperty SmoothnessMaskMapProperty;
        protected MaterialProperty SmoothnessRemapMaxProperty;
        protected MaterialProperty SmoothnessRemapMinProperty;

        protected MaterialProperty SpecularColorProperty;
        protected MaterialProperty SpecularMultiplierProperty;
        protected MaterialProperty SpecularShiftProperty;

        protected MaterialProperty TransmissionColorProperty;
        protected MaterialProperty TransmissionIntensityProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            SmoothnessMaskMapProperty = GetProperty("_SmoothnessMaskMap");
            SmoothnessProperty = GetProperty("_Smoothness");
            SmoothnessRemapMinProperty = GetProperty("_SmoothnessRemapMin");
            SmoothnessRemapMaxProperty = GetProperty("_SmoothnessRemapMax");

            SpecularColorProperty = GetProperty("_SpecularColor");
            SpecularMultiplierProperty = GetProperty("_SpecularMultiplier");
            SpecularShiftProperty = GetProperty("_SpecularShift");
            SecondarySpecularMultiplierProperty = GetProperty("_SecondarySpecularMultiplier");
            SecondarySpecularShiftProperty = GetProperty("_SecondarySpecularShift");

            TransmissionColorProperty = GetProperty("_TransmissionColor");
            TransmissionIntensityProperty = GetProperty("_TransmissionIntensity");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            EditorGUILayout.Space();
            DrawSmoothnessMask(editor);
            EditorGUILayout.Space();
            DrawSpecularProperties(editor);
            EditorGUILayout.Space();
            DrawTransmission(editor);
            EditorGUILayout.Space();
        }

        public void SetKeywords(Material material) => 
            CoreUtils.SetKeyword(material, "_SMOOTHNESS_MASK", SmoothnessMaskMapProperty.textureValue is not null);

        protected virtual void DrawSmoothnessMask(PropertiesEditor editor)
        {
            editor.DrawTexture(HairStyles.SmoothnessMask, SmoothnessMaskMapProperty);

            if (SmoothnessMaskMapProperty.textureValue is not null)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.SmoothnessRemapping, SmoothnessRemapMinProperty,
                    SmoothnessRemapMaxProperty);
            else
                editor.DrawSlider(SurfaceInputsStyles.Smoothness, SmoothnessProperty);

            editor.DrawTextureScaleOffset(SmoothnessMaskMapProperty);
        }

        protected virtual void DrawSpecularProperties(PropertiesEditor editor)
        {
            editor.DrawColor(HairStyles.SpecularColorHair, SpecularColorProperty);
            editor.DrawSlider(HairStyles.SpecularMultiplier, SpecularMultiplierProperty);
            editor.DrawSlider(HairStyles.SpecularShift, SpecularShiftProperty);
            editor.DrawSlider(HairStyles.SecondarySpecularMultiplier, SecondarySpecularMultiplierProperty);
            editor.DrawSlider(HairStyles.SecondarySpecularShift, SecondarySpecularShiftProperty);
        }

        protected virtual void DrawTransmission(PropertiesEditor editor)
        {
            editor.DrawColor(HairStyles.TransmissionColor, TransmissionColorProperty);
            editor.DrawSlider(HairStyles.TransmissionRim, TransmissionIntensityProperty);
        }
    }
}