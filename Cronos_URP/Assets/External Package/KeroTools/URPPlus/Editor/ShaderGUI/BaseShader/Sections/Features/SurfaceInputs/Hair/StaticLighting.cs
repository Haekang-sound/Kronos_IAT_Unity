using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Hair
{
    public class StaticLighting : IDrawable
    {
        protected MaterialProperty StaticLightColorProperty;
        protected MaterialProperty StaticLightVectorProperty;
        protected MaterialProperty StaticSpecularHighlightProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            StaticSpecularHighlightProperty = GetProperty("_StaticSpecularHighlight");
            StaticLightColorProperty = GetProperty("_StaticLightColor");
            StaticLightVectorProperty = GetProperty("_StaticLightVector");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            editor.DrawToggle(HairStyles.StaticHighlight, StaticSpecularHighlightProperty);
            editor.DrawColor(HairStyles.StaticLightColor, StaticLightColorProperty);
            editor.DrawVector4(HairStyles.StaticLightVector, StaticLightVectorProperty);
        }

        public void SetKeywords(Material material)
        {
            var staticSpecularState = StaticSpecularHighlightProperty.floatValue > 0.5f;
            CoreUtils.SetKeyword(material, "_STATICSPECULARHIGHLIGHT_ON", staticSpecularState);
        }
    }
}