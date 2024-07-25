using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class ClearCoat : IDrawable
    {
        private static readonly int ClearCoatMap = Shader.PropertyToID("_ClearCoatMap");
        private static readonly int ClearCoatMask = Shader.PropertyToID("_ClearCoatMask");
        
        protected MaterialProperty ClearCoatMapProperty;
        protected MaterialProperty ClearCoatMaskProperty;
        protected MaterialProperty ClearCoatSmoothnessProperty;

        protected MaterialProperty CoatNormalEnabledProperty;
        protected MaterialProperty CoatNormalMapProperty;
        protected MaterialProperty CoatNormalScaleProperty;

        public void FindProperties(MaterialProperty[] properties)
        {
            ClearCoatMapProperty = GetProperty("_ClearCoatMap");
            ClearCoatMaskProperty = GetProperty("_ClearCoatMask");
            ClearCoatSmoothnessProperty = GetProperty("_ClearCoatSmoothness");

            CoatNormalEnabledProperty = PropertyFinder.FindOptionalProperty("_CoatNormalEnable", properties);
            CoatNormalMapProperty = GetProperty("_CoatNormalMap");
            CoatNormalScaleProperty = GetProperty("_CoatNormalScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            DrawClearCoatMap(editor);

            if (ClearCoatMaskProperty.floatValue <= 0.0f)
                return;

            DrawClearCoatSmoothness(editor);
            DrawClearCoatNormalMap(editor);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(ClearCoatMap))
                CoreUtils.SetKeyword(material, "_CLEARCOATMAP", material.GetTexture(ClearCoatMap));

            if (material.HasProperty(ClearCoatMask))
            {
                var clearCoatState = ClearCoatMaskProperty.floatValue > 0.0f;
                CoreUtils.SetKeyword(material, "_CLEARCOAT", clearCoatState);
            }
        }

        protected virtual void DrawClearCoatMap(PropertiesEditor editor) => 
            editor.DrawTexture(ClearCoatStyles.ClearCoatMask, ClearCoatMapProperty, ClearCoatMaskProperty);

        protected virtual void DrawClearCoatSmoothness(PropertiesEditor editor) => 
            editor.DrawSlider(ClearCoatStyles.ClearCoatSmoothness, ClearCoatSmoothnessProperty);

        protected virtual void DrawClearCoatNormalMap(PropertiesEditor editor)
        {
            var enabled = CoatNormalEnabledProperty.floatValue < ToggleDrawer.BoolTolerance;

            if (enabled)
                return;

            editor.DrawTexture(SurfaceInputsStyles.CoatNormalMap, CoatNormalMapProperty, CoatNormalScaleProperty);
        }
    }
}