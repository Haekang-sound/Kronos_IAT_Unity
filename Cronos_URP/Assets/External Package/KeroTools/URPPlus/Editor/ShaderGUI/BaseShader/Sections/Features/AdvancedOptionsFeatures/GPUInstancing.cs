using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class GPUInstancing : IDrawable
    {
        public virtual void FindProperties(MaterialProperty[] properties)
        {
        }

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawGPUInstancing();

        public void SetKeywords(Material material)
        {
        }
    }
}