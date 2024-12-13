using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class BentNormalMap : IDrawable
    {
        private static readonly int BentNormalMapID = Shader.PropertyToID("_BentNormalMap");
        protected MaterialProperty BentNormalMapProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            BentNormalMapProperty = PropertyFinder.FindOptionalProperty("_BentNormalMap", properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawTexture(SurfaceInputsStyles.BentNormalMap, BentNormalMapProperty);

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(BentNormalMapID))
                CoreUtils.SetKeyword(material, "_BENTNORMALMAP", material.GetTexture(BentNormalMapID));
        }
    }
}