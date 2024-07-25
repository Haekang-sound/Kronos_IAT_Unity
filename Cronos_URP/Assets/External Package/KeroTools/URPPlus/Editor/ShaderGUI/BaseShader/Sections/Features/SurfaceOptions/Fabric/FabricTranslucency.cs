using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions.Fabric
{
    public class FabricTranslucency : IDrawable
    {
        private static readonly int EnableTranslucencyID = Shader.PropertyToID("_EnableTranslucency");
        protected MaterialProperty EnableTranslucencyProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            EnableTranslucencyProperty = PropertyFinder.FindOptionalProperty("_EnableTranslucency", properties);

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawIndented(() =>
            {
                editor.DrawToggle(new GUIContent("Enable Translucency"), EnableTranslucencyProperty);
            });
        }

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(EnableTranslucencyID)) 
                return;
            
            var translucencyState = EnableTranslucencyProperty.floatValue > 0.5f;
            CoreUtils.SetKeyword(material, "_MATERIAL_FEATURE_TRANSLUCENCY", translucencyState);
        }
    }
}