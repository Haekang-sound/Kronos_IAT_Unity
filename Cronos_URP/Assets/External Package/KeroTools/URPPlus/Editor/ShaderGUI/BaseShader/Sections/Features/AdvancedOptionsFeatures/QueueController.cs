using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures
{
    public class QueueController : IDrawable
    {
        private const int QueueOffsetRange = 50;
        protected MaterialProperty QueueOffsetProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            QueueOffsetProperty = PropertyFinder.FindOptionalProperty("_QueueOffset", properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawIntSlider(AdvancedOptionsStyles.QueueSlider, QueueOffsetProperty, -QueueOffsetRange, QueueOffsetRange);

        public void SetKeywords(Material material)
        {
        }
    }
}