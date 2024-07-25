using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class ComplexLitTessellationGUI : ComplexLitGUI
    {
        public override List<Section> SetSections(Material material)
        {
            return new List<Section>
            {
                new SurfaceOptions(SurfaceOptionsFeatures),
                new TessellationOptions(),
                new SurfaceInputs(SurfaceInputsFeatures),
                new DetailInputs(),
                new WeatherInputs(material),
                new TransparencyInputs(material),
                new EmissionInputs(material),
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
        }
    }
}