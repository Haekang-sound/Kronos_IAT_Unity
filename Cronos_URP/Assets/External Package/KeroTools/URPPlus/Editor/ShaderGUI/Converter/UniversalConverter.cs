using KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.HDRP;
using KeroTools.URPPlus.Editor.ShaderGUI.Converter.Shaders.URP;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter
{
    public class UniversalConverter
    {
        private readonly URPLitConverter _urpLitConverter;
        private readonly URPSimpleLitConverter _urpSimpleLitConverter;
        private readonly URPBakedLitConverter _urpBakedLitConverter;
        private readonly HDLitConverter _hdrpLitConverter;
        private readonly HDLayeredLitConverter _hdLayeredLitConverter;

        public UniversalConverter(Material material)
        {
            _urpLitConverter = new URPLitConverter(material);
            _urpSimpleLitConverter = new URPSimpleLitConverter(material);
            _urpBakedLitConverter = new URPBakedLitConverter(material);
            _hdrpLitConverter = new HDLitConverter(material);
            _hdLayeredLitConverter = new HDLayeredLitConverter(material);
        }

        public void UpdateMaterial(Shader oldShader, Shader newShader)
        {
            if(oldShader.name.StartsWith("KeroTools/URP+/"))
                return;
            
            _urpLitConverter.TryUpdateMaterial(oldShader, newShader);
            _urpSimpleLitConverter.TryUpdateMaterial(oldShader, newShader);
            _urpBakedLitConverter.TryUpdateMaterial(oldShader, newShader);
            _hdrpLitConverter.TryUpdateMaterial(oldShader, newShader);
            _hdLayeredLitConverter.TryUpdateMaterial(oldShader, newShader);
        }
    }
}