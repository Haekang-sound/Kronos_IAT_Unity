using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.Converter;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader
{
    public abstract class BaseShaderGUI : UnityEditor.ShaderGUI
    {
        protected MaterialEditor MaterialEditor;
        protected PropertiesEditor PropertiesEditor;

        protected List<Section> Sections = new();
        
        protected static Vector2 IconSize = new(5, 5);

        protected bool FirstTimeApply = true;
        
        protected const string BaseOffsetPropertyName = "_BaseMap";
        
        public virtual void MaterialChanged(Material material) => 
            ValidateMaterial(material);
        
        public override void ValidateMaterial(Material material) => 
            SetKeywords(material);

        public virtual void OnOpenGUI(Material material) => 
            Sections = new List<Section>(SetSections(material));

        public virtual void OnUpdateGUI(Material material) { }

        public virtual List<Section> SetSections(Material material)
        {
            return new List<Section>()
            {
                new SurfaceOptions(material),
                new SurfaceInputs(),
                new AdvancedOptions()
            };
        }

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
        {
            if (materialEditorIn == null)
                throw new ArgumentNullException(nameof(materialEditorIn));

            MaterialEditor = materialEditorIn;
            PropertiesEditor = new PropertiesEditor(MaterialEditor);
            var material = MaterialEditor?.target as Material;
            
            if (material == null)
                return;
            
            if (FirstTimeApply)
            {
                OnOpenGUI(material);
                FirstTimeApply = false;
            }
            
            OnUpdateGUI(material);
            
            // MaterialProperties can be animated so we do not cache them
            // but fetch them every event to ensure animated values are updated correctly
            FindProperties(properties);
            
            RenderSections();
        }
        
        protected virtual void FindProperties(MaterialProperty[] properties)
        {
            if (properties == null)
                return;
            
            foreach (var section in Sections)
                section.FindProperties(properties);
        }
        
        protected virtual void RenderSections()
        {
            foreach (var section in Sections.Where(section => section.IsRendered))
                RenderSection(section);
        }

        protected virtual void RenderSection(Section section)
        {
            EditorGUIUtility.SetIconSize(IconSize);
            using var header = new MaterialHeaderScope(section.Label, (uint)section.Expandable, MaterialEditor);
            if (header.expanded)
                section.Draw(PropertiesEditor);
        }
        
        protected virtual void SetKeywords(Material material)
        {
            foreach (var section in Sections)
                section.SetKeywords(material);
        }
        
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            // Clear all keywords for fresh start
            // Note: this will nuke user-selected custom keywords when they change shaders
            material.shaderKeywords = null;

            base.AssignNewShaderToMaterial(material, oldShader, newShader);
            
            UniversalConverter converter = new(material);
            converter.UpdateMaterial(oldShader, newShader);
        }
    }
}