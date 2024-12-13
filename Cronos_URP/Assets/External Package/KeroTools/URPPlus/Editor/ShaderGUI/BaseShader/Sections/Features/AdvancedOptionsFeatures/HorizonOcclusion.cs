using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class HorizonOcclusion : IDrawable
    {
        private static readonly int HorizonOcclusionID = Shader.PropertyToID("_HorizonOcclusion");
        protected MaterialProperty HorizonFadeProperty;
        protected MaterialProperty HorizonOcclusionProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            HorizonOcclusionProperty = PropertyFinder.FindOptionalProperty("_HorizonOcclusion", properties);
            HorizonFadeProperty = PropertyFinder.FindOptionalProperty("_HorizonFade", properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var occlusionToggle = editor.DrawToggle(AdvancedOptionsStyles.HorizonOcclusion, HorizonOcclusionProperty);

            if (!occlusionToggle)
                return;

            editor.DrawIndented(() => DrawHorizonFade(editor));
        }

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(HorizonOcclusionID)) 
                return;
            
            var horizonOcclusionState = HorizonOcclusionProperty.floatValue > 0.5f;
            CoreUtils.SetKeyword(material, "_HORIZON_SPECULAR_OCCLUSION", horizonOcclusionState);
        }

        protected virtual void DrawHorizonFade(PropertiesEditor editor) => 
            editor.DrawSlider(AdvancedOptionsStyles.HorizonFade, HorizonFadeProperty, 0.0f, 2.0f);
    }
}