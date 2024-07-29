using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.URP
{
    public class URPBakedLitConverter : BaseConverter
    {
        public URPBakedLitConverter(Material material) : base(material) { }
        
        public override void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (oldShader.name != "Universal Render Pipeline/Baked Lit") 
                return;
            
            base.TryUpdateMaterial(oldShader, newShader);
            
            ConvertTexture("_BumpMap", "_NormalMap");
        }
    }
}