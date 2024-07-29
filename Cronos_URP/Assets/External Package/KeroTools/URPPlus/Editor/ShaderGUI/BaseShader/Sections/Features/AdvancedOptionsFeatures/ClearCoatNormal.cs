using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class ClearCoatNormal : IDrawable
    {
        private static readonly int ClearCoatMask = Shader.PropertyToID("_ClearCoatMask");
        private static readonly int CoatNormalEnableID = Shader.PropertyToID("_CoatNormalEnable");
        private readonly Material _material;

        protected MaterialProperty CoatNormalEnabledProperty;

        public ClearCoatNormal(Material material) => 
            _material = material;

        public void FindProperties(MaterialProperty[] properties) => 
            CoatNormalEnabledProperty = PropertyFinder.FindOptionalProperty("_CoatNormalEnable", properties);

        public void Draw(PropertiesEditor editor)
        {
            var clearCoatMask = _material.GetFloat(ClearCoatMask);

            if (clearCoatMask <= 0.0f)
                return;

            editor.DrawToggle(AdvancedOptionsStyles.SecondaryClearCoatNormal, CoatNormalEnabledProperty);
        }

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(CoatNormalEnableID)) 
                return;
            
            var coatNormalState = material.GetFloat(CoatNormalEnableID) > 0.5f;
            CoreUtils.SetKeyword(material, "_COATNORMALMAP", coatNormalState);
        }
    }
}