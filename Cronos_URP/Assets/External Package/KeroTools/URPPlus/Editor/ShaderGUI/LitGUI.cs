using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class LitGUI : BaseShader.BaseShaderGUI
    {
        protected IDrawable[] SurfaceOptionsFeatures;
        protected IDrawable[] SurfaceInputsFeatures;
        protected IDrawable[] AdvancedOptionsFeatures;

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
                new DetailInputs(),
                new WeatherInputs(material),
                new TransparencyInputs(material),
                new EmissionInputs(material),
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
        }
        
        protected IDrawable[] InitializeSurfaceOptions(Material material)
        {
            return new IDrawable[]
            {
                new Workflow(),
                new SurfaceType(material),
                new RenderFace(material),
                new AlphaClipping(),
                new GeometricSpecularAA(),
                new DisplacementType(material)
            };
        }
        
        protected IDrawable[] InitializeSurfaceInputs(Material material)
        {
            return new IDrawable[]
            {
                new BaseMap(),
                new MaskMap(material),
                new SpecularMap(material),
                new NormalMap(),
                new BentNormalMap(),
                new ClearCoat(),
                new HeightMap(material),
                new TextureOffset(BaseOffsetPropertyName)
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
                new ClearCoatNormal(material),
                new HorizonOcclusion(),
                new SpecularOcclusion()
            };
        }
    }
}