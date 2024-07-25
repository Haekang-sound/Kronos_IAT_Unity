using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class BaseMap : IDrawable
    {
        private static readonly int ColorID = Shader.PropertyToID("_Color");
        private static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
        private static readonly int BaseMapID = Shader.PropertyToID("_BaseMap");
        private static readonly int InvTilingScaleID = Shader.PropertyToID("_InvTilingScale");
        
        protected MaterialProperty AlphaMax;
        protected MaterialProperty AlphaMinProperty;
        protected MaterialProperty BaseColorProperty;
        protected MaterialProperty BaseMapProperty;
        protected MaterialProperty InvTilingScaleProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            BaseMapProperty = GetProperty("_BaseMap");
            BaseColorProperty = GetProperty("_BaseColor");
            AlphaMinProperty = GetProperty("_AlphaRemapMin");
            AlphaMax = GetProperty("_AlphaRemapMax");
            InvTilingScaleProperty = GetProperty("_InvTilingScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(SurfaceInputsStyles.BaseMap, BaseMapProperty, BaseColorProperty);

            if (BaseMapProperty.textureValue is not null)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.AlphaRemapping, AlphaMinProperty, AlphaMax);
        }

        public virtual void SetKeywords(Material material)
        {
            if (material.HasProperty(MainTexID))
            {
                material.SetTexture(MainTexID, material.GetTexture(BaseMapID));
                material.SetTextureScale(MainTexID, material.GetTextureScale(BaseMapID));
                material.SetTextureOffset(MainTexID, material.GetTextureOffset(BaseMapID));
            }

            if (material.HasProperty(ColorID))
                material.SetColor(ColorID, material.GetColor(BaseColorID));

            if (material.HasProperty(InvTilingScaleID))
            {
                InvTilingScaleProperty.floatValue = 2.0f / (Mathf.Abs(BaseMapProperty.textureScaleAndOffset.x) 
                                                            + Mathf.Abs(BaseMapProperty.textureScaleAndOffset.y));
            }
        }
    }
}