using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.URP
{
    public class URPSimpleLitConverter : BaseConverter
    {
        public URPSimpleLitConverter(Material material) : base(material) { }
        
        public override void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (oldShader.name != "Universal Render Pipeline/Simple Lit") 
                return;
            
            base.TryUpdateMaterial(oldShader, newShader);
            
            ConvertTexture("_SpecGlossMap", "_MaskMap");
            ConvertColor("_SpecColor", "_SpecularColor");
            ConvertTexture("_BumpMap", "_NormalMap");
        }
    }
}