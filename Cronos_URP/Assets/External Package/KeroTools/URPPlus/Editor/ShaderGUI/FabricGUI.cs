using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions.Fabric;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Fabric;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;
using KeroTools.URPPlus.Editor.ShaderGUI.Converter;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class FabricGUI : BaseShader.BaseShaderGUI
    {
        protected IDrawable[] SurfaceOptionsFeatures;
        protected IDrawable[] SurfaceInputsFeatures;
        protected IDrawable[] AdvancedOptionsFeatures;
        
        private const string ThreadOffsetPropertyName = "_ThreadMap";
        
        public override void OnOpenGUI(Material material)
        {
            SurfaceOptionsFeatures = InitializeSurfaceOptions(material);
            SurfaceInputsFeatures = InitializeSurfaceInputs(material);
            AdvancedOptionsFeatures = InitializeAdvancedOptions(material);
            
            Sections = new List<Section>(SetSections(material));
        }

        public override List<Section> SetSections(Material material)
        {
            return new List<Section>
            {
                new SurfaceOptions(SurfaceOptionsFeatures),
                new SurfaceInputs(SurfaceInputsFeatures),
                new WeatherInputs(material),
                new EmissionInputs(material),
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
        }
        
        private IDrawable[] InitializeSurfaceOptions(Material material)
        {
            return new IDrawable[]
            {
                new SurfaceType(material),
                new RenderFace(material),
                new FabricType(),
                new FabricTranslucency(),
                new AlphaClipping(),
                new GeometricSpecularAA(),
                new DisplacementType(material)
            };
        }
        
        private IDrawable[] InitializeSurfaceInputs(Material material)
        {
            return new IDrawable[]
            {
                new BaseMap(),
                new AnisotropyValue(material),
                new MaskMap(material),
                new SpecularMap(material),
                new NormalMap(),
                new BentNormalMap(),
                new HeightMap(material),
                new DiffusionProfile(material, MaterialTypeMode.Translucency),
                new ThicknessCurvatureMap(material, MaterialTypeMode.Translucency),
                new TextureOffset(BaseOffsetPropertyName),
                new ThreadMap(),
                new TextureOffset(ThreadOffsetPropertyName),
                new FuzzMap()
            };
        }
        
        private IDrawable[] InitializeAdvancedOptions(Material material)
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