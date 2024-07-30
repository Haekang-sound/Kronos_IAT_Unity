using UnityEngine;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit.LayeredFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit
{
    public class Layer : СonstructedSection
    {
        public Layer(Material material, IDrawable[] features, uint layersCount = 1, int layerIndex = 0) : base(features)
        {
            Label = LayeredStyles.Layers[layerIndex];
            Expandable = LayeredStyles.LayerExpandableBits[layerIndex];

            Features = new IDrawable[]
            {
                new LayeringOptions(material, layersCount, layerIndex),
                new LayerBaseMap(layersCount, layerIndex),
                new LayerMaskMap(material, layersCount, layerIndex),
                new LayerNormalMap(layersCount, layerIndex),
                new LayerBentNormal(layersCount, layerIndex),
                new LayerHeightMap(material, layersCount, layerIndex),
                new LayerTextureOffset(layersCount, layerIndex),
                new LayerDetailMap(layersCount, layerIndex)
            };
        }
    }
}