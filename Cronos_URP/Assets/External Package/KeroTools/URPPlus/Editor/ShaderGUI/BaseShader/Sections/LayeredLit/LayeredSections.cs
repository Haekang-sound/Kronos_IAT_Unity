using System.Collections.Generic;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit
{
    public class LayeredSections
    {
        private readonly Material _material;

        public LayeredSections(Material material) => 
            _material = material;

        public Section GetLayer(uint layersCount = 2, int layerIndex = 0) => 
            new Layer(_material, null, layersCount, layerIndex);

        public IEnumerable<Section> GetLayers(uint layersCount = 2)
        {
            var sections = new List<Section>();
            for (var layerIndex = 0; layerIndex < layersCount; layerIndex++)
                sections.Add(new Layer(_material, null, layersCount, layerIndex));

            return sections;
        }
    }
}