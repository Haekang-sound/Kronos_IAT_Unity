using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class HighlightReflections : IDrawable
    {
        private static readonly int SpecularHighlightsID = Shader.PropertyToID("_SpecularHighlights");
        private static readonly int EnvironmentReflectionsID = Shader.PropertyToID("_EnvironmentReflections");
        protected MaterialProperty HighlightsProperty;
        protected MaterialProperty ReflectionsProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            HighlightsProperty = PropertyFinder.FindOptionalProperty("_SpecularHighlights", properties);
            ReflectionsProperty = PropertyFinder.FindOptionalProperty("_EnvironmentReflections", properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawToggle(AdvancedOptionsStyles.Highlights, HighlightsProperty);
            editor.DrawToggle(AdvancedOptionsStyles.Reflections, ReflectionsProperty);
            EditorGUILayout.Space();
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(SpecularHighlightsID))
            {
                var highlightsState = HighlightsProperty.floatValue < 0.5f;
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF", highlightsState);
            }

            if (material.HasProperty(EnvironmentReflectionsID))
            {
                var reflectionsState = ReflectionsProperty.floatValue < 0.5f;
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF", reflectionsState);
            }
        }
    }
}