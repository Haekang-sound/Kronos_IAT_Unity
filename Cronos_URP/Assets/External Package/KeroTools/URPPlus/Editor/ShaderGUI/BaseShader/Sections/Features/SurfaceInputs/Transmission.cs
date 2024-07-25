using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class Transmission : IDrawable
    {
        private static readonly int MaterialTypeId = Shader.PropertyToID("_MaterialType");
        private static readonly int EnableTransmissionID = Shader.PropertyToID("_EnableTransmission");
        private readonly Material _material;
        private readonly MaterialTypeMode? _overrideMode;

        protected MaterialProperty TransmissionScaleProperty;

        public Transmission(Material material, MaterialTypeMode? overrideMode = null)
        {
            _material = material;
            _overrideMode = overrideMode;
        }

        public void FindProperties(MaterialProperty[] properties) => 
            TransmissionScaleProperty = PropertyFinder.FindOptionalProperty("_TransmissionScale", properties);

        public void Draw(PropertiesEditor editor)
        {
            var currentMaterialType = _overrideMode ?? GetMaterialType();
            var transmissionEnabled = _material.GetFloat(EnableTransmissionID) > 0.5f;

            if (currentMaterialType == MaterialTypeMode.SubSurfaceScattering && transmissionEnabled)
                editor.DrawMinFloat(SubSurfaceScatteringStyles.TransmissionScale, TransmissionScaleProperty, 0.0f);
        }

        public void SetKeywords(Material material)
        {
        }

        private MaterialTypeMode GetMaterialType()
        {
            if (_material.HasProperty(MaterialTypeId))
                return (MaterialTypeMode)_material.GetFloat(MaterialTypeId);

            return MaterialTypeMode.Standard;
        }
    }
}