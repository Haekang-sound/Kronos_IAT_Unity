using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Hair;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class HairGUI : BaseShader.BaseShaderGUI
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
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
        }
        
        private IDrawable[] InitializeSurfaceOptions(Material material)
        {
            return new IDrawable[]
            {
                new SurfaceType(material),
                new RenderFace(material),
                new AlphaClipping(),
                new GeometricSpecularAA()
            };
        }
        
        private IDrawable[] InitializeSurfaceInputs(Material material)
        {
            return new IDrawable[]
            {
                new BaseMap(),
                new NormalMap(),
                new AmbientOcclusionMap(),
                new TextureOffset(BaseOffsetPropertyName),
                new SpecularBlock(),
                new StaticLighting()
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
                new GPUInstancing()
            };
        }
    }
}