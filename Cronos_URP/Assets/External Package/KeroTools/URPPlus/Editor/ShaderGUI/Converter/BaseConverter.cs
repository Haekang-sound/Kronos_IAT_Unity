using System;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Converter
{
    public class BaseConverter
    {
        private Material _material;
    
        protected BaseConverter(Material material) =>
            _material = material ? material : throw new ArgumentNullException(nameof(material));
    
        public virtual void TryUpdateMaterial(Shader oldShader, Shader newShader)
        {
            if (_material == null)
                throw new ArgumentNullException(nameof(_material));
    
            _material.shader = newShader;
        }

        protected void ConvertFloat(string oldPropertyName, string newPropertyName)
        {
            if (!_material.HasProperty(oldPropertyName))
                return;
                
            var value = _material.GetFloat(oldPropertyName);
            _material.SetFloat(newPropertyName, value);
        }
        
        protected void ConvertColor(string oldPropertyName, string newPropertyName)
        {
            if (!_material.HasProperty(oldPropertyName))
                return;
            
            var value = _material.GetColor(oldPropertyName);
            _material.SetColor(newPropertyName, value);
        }
        
        protected void ConvertTexture(string oldPropertyName, string newPropertyName)
        {
            var texture = _material.GetTexture(oldPropertyName);

            if (texture != null)
                _material.SetTexture(newPropertyName, texture);
        }
    }
}