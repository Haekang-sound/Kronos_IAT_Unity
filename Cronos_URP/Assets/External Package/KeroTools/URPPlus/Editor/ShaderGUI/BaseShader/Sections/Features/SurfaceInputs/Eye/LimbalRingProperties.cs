using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye
{
    public class LimbalRingProperties : IDrawable
    {
        //protected MaterialProperty LimbalRingColor;
        protected MaterialProperty LimbalRingFadeProperty;
        protected MaterialProperty LimbalRingIntensityProperty;
        protected MaterialProperty LimbalRingSizeIrisProperty;
        protected MaterialProperty LimbalRingSizeScleraProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            LimbalRingSizeIrisProperty = GetProperty("_LimbalRingSizeIris");
            LimbalRingSizeScleraProperty = GetProperty("_LimbalRingSizeSclera");
            LimbalRingFadeProperty = GetProperty("_LimbalRingFade");
            LimbalRingIntensityProperty = GetProperty("_LimbalRingIntensity");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawSlider(new GUIContent("Limbal Ring Size Iris"), LimbalRingSizeIrisProperty);
            editor.DrawSlider(new GUIContent("Limbal RingSizeSclera"), LimbalRingSizeScleraProperty);
            editor.DrawSlider(new GUIContent("Limbal Ring Fade"), LimbalRingFadeProperty);
            editor.DrawSlider(new GUIContent("Limbal Ring Intensity"), LimbalRingIntensityProperty);
        }

        public void SetKeywords(Material material)
        {
        }
    }
}