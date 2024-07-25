using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.HDRP
{
    public class HDLayeredLitConverter : BaseConverter
    {
        public HDLayeredLitConverter(Material material) : base(material) { }
        
        public override void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (oldShader.name != "HDRP/LayeredLit") 
                return;
            
            base.TryUpdateMaterial(oldShader, newShader);
            
            ConvertBase();
            ConvertMaskMap();
            ConvertNormal();
            ConvertHeightMap();
            ConvertDetail();
        }

        private void ConvertBase()
        {
            ConvertColor("_BaseColor0", "_BaseColor");
            ConvertTexture("_BaseColorMap0", "_BaseMap");
        }

        private void ConvertMaskMap()
        {
            ConvertTexture("_MaskMap0", "_MaskMap");
            
            ConvertFloat("_Metallic0", "_Metallic");
            ConvertFloat("_MetallicRemapMin0", "_MetallicRemapMin");
            ConvertFloat("_MetallicRemapMax0", "_MetallicRemapMax");
            
            ConvertFloat("_Smoothness0", "_Smoothness");
            ConvertFloat("_SmoothnessRemapMin0", "_SmoothnessRemapMin");
            ConvertFloat("_SmoothnessRemapMax0", "_SmoothnessRemapMax");
            
            ConvertFloat("_AORemapMin0", "_AORemapMin");
            ConvertFloat("_AORemapMax0", "_AORemapMax");
        }

        private void ConvertNormal()
        {
            ConvertTexture("_NormalMap0", "_NormalMap");
            ConvertFloat("_NormalScale0", "_NormalScale");
            ConvertTexture("_BentNormalMap0", "_BentNormalMap");
        }

        private void ConvertHeightMap()
        {
            ConvertTexture("_HeightMap0", "_HeightMap");
            ConvertFloat("_HeightAmplitude0", "_HeightAmplitude");
            ConvertFloat("_HeightCenter0", "_HeightCenter");
            ConvertFloat("_HeightMapParametrization0", "_HeightMapParametrization");
            ConvertFloat("_HeightOffset0", "_HeightOffset");
            ConvertFloat("_HeightMin0", "_HeightMin");
            ConvertFloat("_HeightMax0", "_HeightMax");
            ConvertFloat("_HeightTessAmplitude0", "_HeightTessAmplitude");
            ConvertFloat("_HeightPoMAmplitude0", "_HeightPoMAmplitude");
        }

        private void ConvertDetail()
        {
            ConvertTexture("_DetailMap0", "_DetailMap");
            ConvertFloat("_DetailAlbedoScale0", "_DetailAlbedoScale");
            ConvertFloat("_DetailNormalScale0", "_DetailNormalScale");
            ConvertFloat("_DetailSmoothnessScale0", "_DetailSmoothnessScale");
        }
    }
}