using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    public class LayeredLitTessellationGUI : LayeredLitGUI
    {
        public override List<Section> SetSections(Material material)
        {
            var sectionsList = new List<Section>
            {
                new SurfaceOptions(SurfaceOptionsFeatures),
                new TessellationOptions(),
                new SurfaceInputs(SurfaceInputsFeatures),
                new WeatherInputs(material),
                new EmissionInputs(material),
                new AdvancedOptions(AdvancedOptionsFeatures)
            };
            
            //Hardcoded insert of LayersSections after SurfaceInputs
            var layeredSections = new LayeredSections(material).GetLayers(PreviousLayerCount);
            sectionsList.InsertRange(3, layeredSections);
            
            return sectionsList;
        }
    }
}