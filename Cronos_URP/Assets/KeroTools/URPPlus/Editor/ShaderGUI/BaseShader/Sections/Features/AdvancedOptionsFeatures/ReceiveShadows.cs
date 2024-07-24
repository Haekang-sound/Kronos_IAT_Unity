using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class ReceiveShadows : IDrawable
    {
        private static readonly int ReceiveShadowsID = Shader.PropertyToID("_ReceiveShadows");
        protected MaterialProperty ReceiveShadowsProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            ReceiveShadowsProperty = PropertyFinder.FindOptionalProperty("_ReceiveShadows", properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawToggle(AdvancedOptionsStyles.ReceiveShadow, ReceiveShadowsProperty);

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(ReceiveShadowsID)) 
                return;
            
            var receiveShadowsState = ReceiveShadowsProperty.floatValue < 0.5f;
            CoreUtils.SetKeyword(material, "_RECEIVE_SHADOWS_OFF", receiveShadowsState);
        }
    }
}