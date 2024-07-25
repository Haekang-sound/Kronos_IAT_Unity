using System;
using System.Collections.Generic;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class MaterialType : IDrawable
    {
        private static readonly int MaterialTypeID = Shader.PropertyToID("_MaterialType");
        private static readonly int DiffusionLutID = Shader.PropertyToID("_DiffusionLUT");
        private static readonly int EnableTransmissionID = Shader.PropertyToID("_EnableTransmission");

        private static readonly Dictionary<MaterialTypeMode, string> KeywordMap = new()
        {
            { MaterialTypeMode.SubSurfaceScattering, "_MATERIAL_FEATURE_SUBSURFACE_SCATTERING" },
            { MaterialTypeMode.Anisotropy, "_MATERIAL_FEATURE_ANISOTROPY" },
            { MaterialTypeMode.Iridescence, "_MATERIAL_FEATURE_IRIDESCENCE" },
            { MaterialTypeMode.Translucency, "_MATERIAL_FEATURE_TRANSLUCENCY" }
        };

        protected readonly string[] MaterialTypeOptions = Enum.GetNames(typeof(MaterialTypeMode));
        
        protected MaterialProperty EnableTransmissionProperty;
        protected MaterialProperty MaterialTypeProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            MaterialTypeProperty = PropertyFinder.FindOptionalProperty("_MaterialType", properties);
            EnableTransmissionProperty = PropertyFinder.FindOptionalProperty("_EnableTransmission", properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawPopup(SurfaceOptionsStyles.MaterialID, MaterialTypeProperty, MaterialTypeOptions);

            var materialTypeValue = (MaterialTypeMode)MaterialTypeProperty.floatValue;
            var subsurfaceScatteringMode = materialTypeValue == MaterialTypeMode.SubSurfaceScattering;

            if (!subsurfaceScatteringMode)
                return;

            editor.DrawIndented(() =>
            {
                editor.DrawToggle(SurfaceOptionsStyles.EnableTransmission, EnableTransmissionProperty);
            });
        }

        public void SetKeywords(Material material)
        {
            foreach (var keyword in KeywordMap.Values) CoreUtils.SetKeyword(material, keyword, false);

            if (material.HasProperty(MaterialTypeID))
            {
                var materialTypeMode = (MaterialTypeMode)material.GetFloat(MaterialTypeID);

                if (KeywordMap.TryGetValue(materialTypeMode, out var keyword))
                    CoreUtils.SetKeyword(material, keyword, true);

                if (material.HasProperty(DiffusionLutID))
                    CoreUtils.SetKeyword(material, "_DIFFUSION_LUT", material.GetTexture(DiffusionLutID));

                if (material.HasProperty(EnableTransmissionID))
                {
                    var transmissionState = material.GetFloat(EnableTransmissionID) > 0.5f;
                    CoreUtils.SetKeyword(material, "_MATERIAL_FEATURE_TRANSMISSION", transmissionState);
                }
            }
        }
    }
}