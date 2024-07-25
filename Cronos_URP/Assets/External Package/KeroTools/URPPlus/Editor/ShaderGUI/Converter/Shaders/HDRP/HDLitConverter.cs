using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.HDRP
{
    public class HDLitConverter : BaseConverter
    {
        public HDLitConverter(Material material) : base(material) { }
        
        public override void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (oldShader.name != "HDRP/Lit") 
                return;
            
            base.TryUpdateMaterial(oldShader, newShader);
            
            ConvertTexture("_BaseColorMap", "_BaseMap");
            ConvertTexture("_CoatMaskMap", "_ClearCoatMap");
            ConvertFloat("_CoatMask", "_ClearCoatMask");
            ConvertFloat("_CoatMask", "_ClearCoatSmoothness");
            ConvertColor("_EmissiveColor", "_EmissionColor");
            ConvertTexture("_EmissiveColorMap", "_EmissionMap");
        }
    }
}