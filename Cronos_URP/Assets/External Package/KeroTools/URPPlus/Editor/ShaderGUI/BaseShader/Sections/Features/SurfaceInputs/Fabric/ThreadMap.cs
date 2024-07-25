using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Fabric
{
    public class ThreadMap : IDrawable
    {
        private static readonly int ThreadMapID = Shader.PropertyToID("_ThreadMap");
        protected MaterialProperty ThreadAOScale;
        protected MaterialProperty ThreadMapProperty;
        protected MaterialProperty ThreadNormalScaleProperty;
        protected MaterialProperty ThreadSmoothnessScaleProperty;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            ThreadMapProperty = GetProperty("_ThreadMap");
            ThreadAOScale = GetProperty("_ThreadAOScale");
            ThreadNormalScaleProperty = GetProperty("_ThreadNormalScale");
            ThreadSmoothnessScaleProperty = GetProperty("_ThreadSmoothnessScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            editor.DrawTexture(new GUIContent("Thread Map"), ThreadMapProperty);

            if (ThreadMapProperty.textureValue is null)
                return;

            editor.DrawSlider(new GUIContent("ThreadAOScale"), ThreadAOScale);
            editor.DrawSlider(new GUIContent("Thread Normal Scale"), ThreadNormalScaleProperty);
            editor.DrawSlider(new GUIContent("Thread Smoothness Scale"), ThreadSmoothnessScaleProperty);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(ThreadMapID))
                CoreUtils.SetKeyword(material, "_THREADMAP", material.GetTexture(ThreadMapID));
        }
    }
}