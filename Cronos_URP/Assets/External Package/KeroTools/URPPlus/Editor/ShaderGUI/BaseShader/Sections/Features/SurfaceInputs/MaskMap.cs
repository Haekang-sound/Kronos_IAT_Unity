using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class MaskMap : IDrawable
    {
        private static readonly int WorkflowID = Shader.PropertyToID("_WorkflowMode");
        private static readonly int MaskMapID = Shader.PropertyToID("_MaskMap");
        private readonly Material _material;
        
        protected MaterialProperty MaskMapProperty;
        protected MaterialProperty MetallicProperty;
        protected MaterialProperty MetallicMaxProperty;
        protected MaterialProperty MetallicMinProperty;
        protected MaterialProperty SmoothnessProperty;
        protected MaterialProperty SmoothnessMaxProperty;
        protected MaterialProperty SmoothnessMinProperty;
        protected MaterialProperty AOMaxProperty;
        protected MaterialProperty AOMinProperty;

        public MaskMap(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            MaskMapProperty = GetProperty("_MaskMap");

            MetallicProperty = GetProperty("_Metallic");
            SmoothnessProperty = GetProperty("_Smoothness");

            MetallicMinProperty = GetProperty("_MetallicRemapMin");
            MetallicMaxProperty = GetProperty("_MetallicRemapMax");
            SmoothnessMinProperty = GetProperty("_SmoothnessRemapMin");
            SmoothnessMaxProperty = GetProperty("_SmoothnessRemapMax");
            AOMinProperty = GetProperty("_AORemapMin");
            AOMaxProperty = GetProperty("_AORemapMax");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var workflowMode = GetWorkflow();
            var isMetallic = workflowMode == WorkflowMode.Metallic;

            DrawRemapSliders(editor, isMetallic);
            DrawMaskMap(editor, isMetallic);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(MaskMapID))
                CoreUtils.SetKeyword(material, "_MASKMAP", material.GetTexture(MaskMapID));
        }

        protected virtual void DrawMaskMap(PropertiesEditor editor, bool isMetallic)
        {
            var maskMapContent = isMetallic ? SurfaceInputsStyles.MaskMap : SurfaceInputsStyles.MaskMapSpecular;

            editor.DrawTexture(maskMapContent, MaskMapProperty);
        }

        protected virtual void DrawRemapSliders(PropertiesEditor editor, bool isMetallic)
        {
            if (HasMaskMap())
                DrawRemapping(editor, isMetallic);
            else
                DrawMetallicSmoothness(editor, isMetallic);
        }

        protected virtual void DrawRemapping(PropertiesEditor editor, bool isMetallic)
        {
            if (isMetallic)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.MetallicRemapping, MetallicMinProperty, MetallicMaxProperty);

            editor.MinMaxShaderProperty(SurfaceInputsStyles.SmoothnessRemapping, SmoothnessMinProperty, SmoothnessMaxProperty);
            editor.MinMaxShaderProperty(SurfaceInputsStyles.AORemapping, AOMinProperty, AOMaxProperty);
        }

        protected virtual void DrawMetallicSmoothness(PropertiesEditor editor, bool isMetallic)
        {
            if (isMetallic)
                editor.DrawSlider(SurfaceInputsStyles.Metallic, MetallicProperty);

            editor.DrawSlider(SurfaceInputsStyles.Smoothness, SmoothnessProperty);
        }

        private WorkflowMode GetWorkflow()
        {
            if (!HasWorkflow())
                return WorkflowMode.Specular;

            return (WorkflowMode)_material.GetFloat(WorkflowID);
        }

        private bool HasMaskMap()
        {
            if (!HasMaskMapProperty())
                return false;

            return MaskMapProperty.textureValue is not null;
        }

        private bool HasWorkflow() => 
            _material.HasProperty(WorkflowID);

        private bool HasMaskMapProperty() => 
            _material.HasProperty(MaskMapID);
    }
}