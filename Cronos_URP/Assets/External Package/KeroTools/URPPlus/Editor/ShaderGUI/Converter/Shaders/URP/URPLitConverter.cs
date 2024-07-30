using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.URP
{
    public class URPLitConverter : BaseConverter
    {
        public URPLitConverter(Material material) : base(material) { }
        
        public override void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (oldShader.name != "Universal Render Pipeline/Lit" && oldShader.name != "Universal Render Pipeline/Complex Lit") 
                return;
            
            base.TryUpdateMaterial(oldShader, newShader);
            
            ConvertTexture("_MetallicGlossMap", "_MaskMap");
            ConvertTexture("_SpecGlossMap", "_SpecularColorMap");
            ConvertColor("_SpecColor", "_SpecularColor");
            ConvertTexture("_BumpMap", "_NormalMap");
            ConvertFloat("_BumpScale", "_NormalScale");
            ConvertTexture("_ParallaxMap", "_HeightMap");
            ConvertTexture("_DetailNormalMap", "_DetailMap");
        }
    }
}