using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions
{
    public class Workflow : IDrawable
    {
        private static readonly int WorkflowModeID = Shader.PropertyToID("_WorkflowMode");
        protected readonly string[] WorkflowModeOptions = Enum.GetNames(typeof(WorkflowMode));
        
        protected MaterialProperty WorkflowModeProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            WorkflowModeProperty = PropertyFinder.FindOptionalProperty("_WorkflowMode", properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawPopup(SurfaceOptionsStyles.WorkflowMode, WorkflowModeProperty, WorkflowModeOptions);

        public void SetKeywords(Material material)
        {
            var isSpecularWorkflow = false;
            if (material.HasProperty(WorkflowModeID))
                isSpecularWorkflow = (WorkflowMode)material.GetFloat(WorkflowModeID) == WorkflowMode.Specular;

            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkflow);
        }
    }
}