using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye
{
    public class PupilProperties : IDrawable
    {
        protected MaterialProperty MaximalPupilApertureProperty;
        protected MaterialProperty MinimalPupilApertureProperty;
        protected MaterialProperty PupilApertureProperty;
        protected MaterialProperty PupilRadiusProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            PupilRadiusProperty = GetProperty("_PupilRadius");
            PupilApertureProperty = GetProperty("_PupilAperture");
            MinimalPupilApertureProperty = GetProperty("_MinimalPupilAperture");
            MaximalPupilApertureProperty = GetProperty("_MaximalPupilAperture");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawSlider(new GUIContent("Pupil Radius"), PupilRadiusProperty);
            editor.DrawSlider(new GUIContent("Pupil Aperture"), PupilApertureProperty);
            editor.DrawSlider(new GUIContent("Minimal Pupil Aperture"), MinimalPupilApertureProperty);
            editor.DrawSlider(new GUIContent("Maximal Pupil Aperture"), MaximalPupilApertureProperty);
        }

        public void SetKeywords(Material material)
        {
        }
    }
}