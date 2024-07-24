using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures
{
    public class LayerMaskMap : LayerFeature
    {
        private readonly Material _material;
        private readonly uint _layersCount;
        private readonly int _layerIndex;

        private const string MaskMap = "_MaskMap";
        private const string Metallic = "_Metallic";
        private const string Smoothness = "_Smoothness";
        private const string MetallicRemapMin = "_MetallicRemapMin";
        private const string MetallicRemapMax = "_MetallicRemapMax";
        private const string SmoothnessRemapMin = "_SmoothnessRemapMin";
        private const string SmoothnessRemapMax = "_SmoothnessRemapMax";
        private const string AORemapMin = "_AORemapMin";
        private const string AORemapMax = "_AORemapMax";

        protected MaterialProperty[] MaskMapProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] MetallicProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] SmoothnessProperties = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] MetallicMin = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] MetallicMax = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] SmoothnessMin = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] SmoothnessMax = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] AOMin = new MaterialProperty[MaxLayersCount];
        protected MaterialProperty[] AOMax = new MaterialProperty[MaxLayersCount];

        private static readonly int WorkflowID = Shader.PropertyToID("_WorkflowMode");

        public LayerMaskMap(Material material, uint layersCount, int layerIndex)
        {
            _material = material;
            _layersCount = layersCount;
            _layerIndex = layerIndex;
        }

        public override void FindProperties(MaterialProperty[] properties)
        {
            MaskMapProperties = GetLayerProperty(MaskMap);

            MetallicProperties = GetLayerProperty(Metallic);
            SmoothnessProperties = GetLayerProperty(Smoothness);

            MetallicMin = GetLayerProperty(MetallicRemapMin);
            MetallicMax = GetLayerProperty(MetallicRemapMax);
            SmoothnessMin = GetLayerProperty(SmoothnessRemapMin);
            SmoothnessMax = GetLayerProperty(SmoothnessRemapMax);
            AOMin = GetLayerProperty(AORemapMin);
            AOMax = GetLayerProperty(AORemapMax);
            return;

            MaterialProperty[] GetLayerProperty(string propertyName) => 
                LayerUtils.FindLayerProperty(propertyName, properties, _layersCount);
        }

        public override void Draw(PropertiesEditor editor)
        {
            var workflowMode = GetWorkflow();
            var isMetallic = workflowMode == WorkflowMode.Metallic;

            DrawRemapSliders(editor, isMetallic);
            DrawMaskMap(editor, isMetallic);
        }

        protected virtual void DrawMaskMap(PropertiesEditor editor, bool isMetallic)
        {
            var maskMapContent = isMetallic ? SurfaceInputsStyles.MaskMap : SurfaceInputsStyles.MaskMapSpecular;

            editor.DrawTexture(maskMapContent, MaskMapProperties[_layerIndex]);
        }

        protected virtual void DrawRemapSliders(PropertiesEditor editor, bool isMetallic)
        {
            if (HasMaskMap())
                DrawRemapping(editor, isMetallic);
            else
                DrawMetallicSmoothness(editor, isMetallic);
        }

        protected virtual void DrawRemapping(PropertiesEditor editor, bool isMetallic)
        {
            if (isMetallic)
                editor.MinMaxShaderProperty(SurfaceInputsStyles.MetallicRemapping, MetallicMin[_layerIndex], MetallicMax[_layerIndex]);

            editor.MinMaxShaderProperty(SurfaceInputsStyles.SmoothnessRemapping, SmoothnessMin[_layerIndex], SmoothnessMax[_layerIndex]);
            editor.MinMaxShaderProperty(SurfaceInputsStyles.AORemapping, AOMin[_layerIndex], AOMax[_layerIndex]);
        }

        protected virtual void DrawMetallicSmoothness(PropertiesEditor editor, bool isMetallic)
        {
            if (isMetallic)
                editor.DrawSlider(SurfaceInputsStyles.Metallic, MetallicProperties[_layerIndex]);

            editor.DrawSlider(SurfaceInputsStyles.Smoothness, SmoothnessProperties[_layerIndex]);
        }

        private WorkflowMode GetWorkflow()
        {
            if (!_material.HasProperty(WorkflowID))
                return WorkflowMode.Specular;

            return (WorkflowMode)_material.GetFloat(WorkflowID);
        }

        private bool HasMaskMap()
        {
            var layerMaskMapID = GetLayerMaskMapID();
            if (!_material.HasProperty(layerMaskMapID))
                return false;

            return MaskMapProperties[_layerIndex].textureValue is not null;
        }

        public override void SetKeywords(Material material)
        {
            var layerMaskMapID = GetLayerMaskMapID();
            
            if (!material.HasProperty(layerMaskMapID)) 
                return;
            
            var hasLayerMaskMap = material.GetTexture(layerMaskMapID);
            CoreUtils.SetKeyword(material, LayerUtils.LayerProperty("_MASKMAP", _layerIndex), hasLayerMaskMap);
        }

        private string GetLayerMaskMapID() => 
            LayerUtils.LayerProperty(MaskMap, _layerIndex);
    }
}