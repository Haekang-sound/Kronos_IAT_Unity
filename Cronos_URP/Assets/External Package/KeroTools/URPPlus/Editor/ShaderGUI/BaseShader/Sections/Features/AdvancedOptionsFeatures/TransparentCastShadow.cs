using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class TransparentCastShadow : IDrawable
    {
        private static readonly int SurfaceID = Shader.PropertyToID("_Surface");
        private static readonly int CastShadowsID = Shader.PropertyToID("_CastShadows");
        private readonly Material _material;

        protected MaterialProperty CastShadowsProperty;

        public TransparentCastShadow(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            CastShadowsProperty = PropertyFinder.FindOptionalProperty("_CastShadows", properties);

        public virtual void Draw(PropertiesEditor editor)
        {
            var surfaceTypeValue = _material.GetFloat(SurfaceID);
            var isTransparent = (SurfaceTypeMode)surfaceTypeValue == SurfaceTypeMode.Transparent;

            if (!isTransparent)
                return;

            editor.DrawToggle(AdvancedOptionsStyles.CastShadow, CastShadowsProperty);
        }

        public void SetKeywords(Material material)
        {
            var castShadow = true;
            if (material.HasProperty(SurfaceID) && material.HasProperty(CastShadowsID))
            {
                if ((SurfaceTypeMode)material.GetFloat(SurfaceID) == SurfaceTypeMode.Transparent)
                    castShadow = material.GetFloat(CastShadowsID) > 0.5f;
            }
            else
            {
                castShadow = IsOpaque(material);
            }

            material.SetShaderPassEnabled("ShadowCaster", castShadow);
        }

        private bool IsOpaque(Material material)
        {
            var opaque = true;

            if (material.HasProperty(SurfaceID))
                opaque = (SurfaceTypeMode)material.GetFloat(SurfaceID) == SurfaceTypeMode.Opaque;

            return opaque;
        }
    }
}