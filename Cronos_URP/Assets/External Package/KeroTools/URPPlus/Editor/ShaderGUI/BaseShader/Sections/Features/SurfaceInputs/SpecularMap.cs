using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class SpecularMap : IDrawable
    {
        private static readonly int WorkflowId = Shader.PropertyToID("_WorkflowMode");
        private readonly Material _material;
        
        protected MaterialProperty SpecularColorProperty;
        protected MaterialProperty SpecularMapProperty;

        public SpecularMap(Material material)
        {
            _material = material;
        }

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            SpecularMapProperty = PropertyFinder.FindOptionalProperty("_SpecularColorMap", properties);
            SpecularColorProperty = PropertyFinder.FindOptionalProperty("_SpecularColor", properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var workflowMode = GetWorkflow();
            var isMetallic = workflowMode == WorkflowMode.Metallic;

            if (isMetallic)
                return;

            editor.DrawTexture(SurfaceInputsStyles.SpecularColor, SpecularMapProperty, SpecularColorProperty);
        }

        public void SetKeywords(Material material)
        {
        }

        private WorkflowMode GetWorkflow()
        {
            if (!HasWorkflow())
                return WorkflowMode.Specular;

            return (WorkflowMode)_material.GetFloat(WorkflowId);
        }

        private bool HasWorkflow() => 
            _material.HasProperty(WorkflowId);
    }
}