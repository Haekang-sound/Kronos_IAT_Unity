using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class ThicknessCurvatureMap : IDrawable
    {
        private static readonly int MaterialTypeId = Shader.PropertyToID("_MaterialType");
        private static readonly int ThicknessCurvatureMapID = Shader.PropertyToID("_ThicknessCurvatureMap");
        
        private readonly Material _material;
        private readonly MaterialTypeMode? _overrideMode;
        
        protected MaterialProperty CurvatureProperty;
        protected MaterialProperty ThicknessProperty;

        protected MaterialProperty ThicknessCurvatureMapProperty;
        protected MaterialProperty ThicknessCurvatureRemapProperty;

        public ThicknessCurvatureMap(Material material, MaterialTypeMode? overrideMode = null)
        {
            _material = material;
            _overrideMode = overrideMode;
        }

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            ThicknessCurvatureMapProperty = GetProperty("_ThicknessCurvatureMap");
            ThicknessProperty = GetProperty("_Thickness");
            CurvatureProperty = GetProperty("_Curvature");
            ThicknessCurvatureRemapProperty = GetProperty("_ThicknessCurvatureRemap");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var currentMaterialType = _overrideMode ?? GetMaterialType();

            if (currentMaterialType == MaterialTypeMode.SubSurfaceScattering)
                DrawThicknessCurvature(editor);
            else if (currentMaterialType == MaterialTypeMode.Translucency) DrawThickness(editor);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(ThicknessCurvatureMapID))
                CoreUtils.SetKeyword(material, "_THICKNESS_CURVATUREMAP", material.GetTexture(ThicknessCurvatureMapID));
        }

        protected virtual void DrawThickness(PropertiesEditor editor)
        {
            editor.DrawTexture(SubSurfaceScatteringStyles.ThicknessMap, ThicknessCurvatureMapProperty);

            if (ThicknessCurvatureMapProperty.textureValue == null)
                editor.DrawSlider(SubSurfaceScatteringStyles.Thickness, ThicknessProperty);
            else
                editor.MinMaxShaderPropertyXY(SubSurfaceScatteringStyles.ThicknessRemap, ThicknessCurvatureRemapProperty);
        }

        protected virtual void DrawThicknessCurvature(PropertiesEditor editor)
        {
            editor.DrawTexture(SubSurfaceScatteringStyles.ThicknessCurvatureMap, ThicknessCurvatureMapProperty);

            if (ThicknessCurvatureMapProperty.textureValue == null)
            {
                editor.DrawSlider(SubSurfaceScatteringStyles.Thickness, ThicknessProperty);
                editor.DrawSlider(SubSurfaceScatteringStyles.Curvature, CurvatureProperty);
            }
            else
            {
                editor.MinMaxShaderPropertyXY(SubSurfaceScatteringStyles.ThicknessRemap, ThicknessCurvatureRemapProperty);
                editor.MinMaxShaderPropertyZW(SubSurfaceScatteringStyles.CurvatureRemap, ThicknessCurvatureRemapProperty);
            }
        }

        private MaterialTypeMode GetMaterialType()
        {
            if (_material.HasProperty(MaterialTypeId))
                return (MaterialTypeMode)_material.GetFloat(MaterialTypeId);

            return MaterialTypeMode.Standard;
        }
    }
}