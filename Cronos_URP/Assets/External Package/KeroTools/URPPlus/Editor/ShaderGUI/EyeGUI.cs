using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs.Eye;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class EyeGUI : BaseShader.BaseShaderGUI
    {
        protected IDrawable[] SurfaceOptionsFeatures;
        protected IDrawable[] SurfaceInputsFeatures;
        protected IDrawable[] AdvancedOptionsFeatures;

        public override void OnOpenGUI(Material material)
        {
            SurfaceOptionsFeatures = InitializeSurfaceOptions(material);
            SurfaceInputsFeatures = InitializeSurfaceInputs();
            AdvancedOptionsFeatures = InitializeAdvancedOptions(material);
            
            Sections = new List<Section>(SetSections(material));
        }

        public override List<Section> SetSections(Material material)
        {
            return new List<Section>
            {
                new SurfaceOptions(SurfaceOptionsFeatures),
                new SurfaceInputs(SurfaceInputsFeatures),
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
                new GeometricSpecularAA()
            };
        }
        
        private IDrawable[] InitializeSurfaceInputs()
        {
            return new IDrawable[]
            {
                new Sclera(),
                new Iris(),
                new PupilProperties(),
                new LimbalRingProperties(),
                new GeometryProperties()
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
            };
        }
    }
}