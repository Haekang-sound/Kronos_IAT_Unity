using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class Iridescence : IDrawable
    {
        private static readonly int MaterialTypeId = Shader.PropertyToID("_MaterialType");
        private static readonly int IridescenceThicknessMapID = Shader.PropertyToID("_IridescenceThicknessMap");
        private readonly Material _material;

        protected MaterialProperty IridescenceLUTProperty;
        protected MaterialProperty IridescenceMaskMapProperty;
        protected MaterialProperty IridescenceMaskScaleProperty;
        protected MaterialProperty IridescenceShiftProperty;
        protected MaterialProperty IridescenceThicknessProperty;
        protected MaterialProperty IridescenceThicknessMapProperty;
        protected MaterialProperty IridescenceThicknessRemapProperty;

        public Iridescence(Material material) => 
            _material = material;

        public void FindProperties(MaterialProperty[] properties)
        {
            IridescenceLUTProperty = GetProperty("_IridescenceLUT");
            IridescenceShiftProperty = GetProperty("_IridescenceShift");
            IridescenceThicknessMapProperty = GetProperty("_IridescenceThicknessMap");
            IridescenceThicknessProperty = GetProperty("_IridescenceThickness");
            IridescenceThicknessRemapProperty = GetProperty("_IridescenceThicknessRemap");
            IridescenceMaskMapProperty = GetProperty("_IridescenceMaskMap");
            IridescenceMaskScaleProperty = GetProperty("_IridescenceMaskScale");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public void Draw(PropertiesEditor editor)
        {
            var currentMaterialType = GetMaterialType();

            if (currentMaterialType != MaterialTypeMode.Iridescence)
                return;

            DrawIridescenceLUT(editor);
            DrawIridescenceMask(editor);
            DrawIridescenceThickness(editor);
        }

        public void SetKeywords(Material material)
        {
            if (material.HasProperty(IridescenceThicknessMapID))
                CoreUtils.SetKeyword(material, "_IRIDESCENCE_THICKNESSMAP",
                    material.GetTexture(IridescenceThicknessMapID));
        }

        protected virtual void DrawIridescenceLUT(PropertiesEditor editor)
        {
            if (URPPlusSettings.useIridescenceLUT)
                editor.DrawTexture(IridescenceStyles.IridescenceLut, IridescenceLUTProperty, IridescenceShiftProperty);
        }

        protected virtual void DrawIridescenceMask(PropertiesEditor editor) => 
            editor.DrawTexture(IridescenceStyles.IridescenceMask, IridescenceMaskMapProperty, IridescenceMaskScaleProperty);

        protected virtual void DrawIridescenceThickness(PropertiesEditor editor)
        {
            if (IridescenceThicknessMapProperty.textureValue != null)
            {
                editor.DrawTexture(IridescenceStyles.IridescenceThicknessMap, IridescenceThicknessMapProperty);
                editor.MinMaxShaderPropertyXY(IridescenceStyles.IridescenceThicknessRemap, IridescenceThicknessRemapProperty);
            }
            else
            {
                editor.DrawTexture(IridescenceStyles.IridescenceThicknessMap, IridescenceThicknessMapProperty,
                    IridescenceThicknessProperty);
            }
        }

        private MaterialTypeMode GetMaterialType()
        {
            if (_material.HasProperty(MaterialTypeId))
                return (MaterialTypeMode)_material.GetFloat(MaterialTypeId);

            return MaterialTypeMode.Standard;
        }
    }
}