        using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class LayeredLitGUI : BaseShader.BaseShaderGUI
    {
        protected IDrawable[] SurfaceOptionsFeatures;
        protected IDrawable[] SurfaceInputsFeatures;
        protected IDrawable[] AdvancedOptionsFeatures;

        protected uint PreviousLayerCount = 2;
        private static readonly int LayerCountID = Shader.PropertyToID("_LayerCount");

        public override void OnOpenGUI(Material material)
        {
            SurfaceOptionsFeatures = InitializeSurfaceOptions(material);
            SurfaceInputsFeatures = InitializeSurfaceInputs();
            AdvancedOptionsFeatures = InitializeAdvancedOptions(material);
            
            PreviousLayerCount = (uint)material.GetFloat(LayerCountID);
            Sections = new List<Section>(SetSections(material));
        }
        
        public override void OnUpdateGUI(Material material)
        {
            var currentLayerCount = (uint)material.GetFloat(LayerCountID);
            
            if (currentLayerCount == PreviousLayerCount) 
                return;
            
            PreviousLayerCount = currentLayerCount;
            Sections = SetSections(material);
        }

        public override List<Section> SetSections(Material material)
        {
            var sectionsList = new List<Section>
            {
                new SurfaceOptions(SurfaceOptionsFeatures),
                new SurfaceInputs(SurfaceInputsFeatures),
                new WeatherInputs(material),
                new EmissionInputs(material),
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
            
            //Hardcoded insert of LayersSections after SurfaceInputs
            var layeredSections = new LayeredSections(material).GetLayers(PreviousLayerCount);
            sectionsList.InsertRange(2, layeredSections);
            
            return sectionsList;
        }
        
        protected IDrawable[] InitializeSurfaceOptions(Material material)
        {
            return new IDrawable[]
            {
                new SurfaceType(material),
                new RenderFace(material),
                new AlphaClipping(),
                new GeometricSpecularAA(),
                new DisplacementType(material)
            };
        }
        
        protected IDrawable[] InitializeSurfaceInputs()
        {
            return new IDrawable[]
            {
                new LayeredLitSurfaceInputs()
            };
        }

        protected IDrawable[] InitializeAdvancedOptions(Material material)
        {
            return new IDrawable[]
            {
                new TransparentCastShadow(material),
                new ReceiveShadows(),
                new HighlightReflections(),
                new QueueController(),
                new GPUInstancing(),
                new HorizonOcclusion(),
                new SpecularOcclusion()
            };
        }
    }
}