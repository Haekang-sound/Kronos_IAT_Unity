using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Fabric
{
    public class AnisotropyValue : IDrawable
    {
        private static readonly int FabricType = Shader.PropertyToID("_FabricType");
        private readonly Material _material;

        protected MaterialProperty AnisotropyProperty;

        public AnisotropyValue(Material material) => 
            _material = material;

        public void FindProperties(MaterialProperty[] properties) => 
            AnisotropyProperty = PropertyFinder.FindOptionalProperty("_Anisotropy", properties);

        public void Draw(PropertiesEditor editor)
        {
            var fabricType = GetFabricType();

            if (fabricType != FabricMaterialType.Silk)
                return;

            editor.DrawSlider(AnisotropyStyles.Anisotropy, AnisotropyProperty);
        }

        public void SetKeywords(Material material)
        {
        }

        private FabricMaterialType GetFabricType()
        {
            if (!HasFabric())
                return FabricMaterialType.Silk;

            return (FabricMaterialType)_material.GetFloat(FabricType);
        }

        private bool HasFabric() => 
            _material.HasProperty(FabricType);
    }
}