using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions.Fabric
{
    public class FabricType : IDrawable
    {
        private static readonly int FabricTypeID = Shader.PropertyToID("_FabricType");
        protected readonly string[] FabricTypeOptions = Enum.GetNames(typeof(FabricMaterialType));
        
        protected MaterialProperty FabricTypeProperty;

        public virtual void FindProperties(MaterialProperty[] properties) => 
            FabricTypeProperty = PropertyFinder.FindOptionalProperty("_FabricType", properties);

        public virtual void Draw(PropertiesEditor editor) => 
            editor.DrawPopup(SurfaceOptionsStyles.MaterialID, FabricTypeProperty, FabricTypeOptions);

        public void SetKeywords(Material material)
        {
            if (!material.HasProperty(FabricTypeID)) 
                return;
            
            var fabricTypeMode = (FabricMaterialType)material.GetFloat(FabricTypeID);

            if (fabricTypeMode == FabricMaterialType.CottonWool)
                material.EnableKeyword("_MATERIAL_FEATURE_SHEEN");
            else
                material.DisableKeyword("_MATERIAL_FEATURE_SHEEN");
        }
    }
}