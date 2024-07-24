using UnityEngine;
using UnityEditor;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceOptions;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class SurfaceOptions : СonstructedSection
    {
        public SurfaceOptions(Material material) : this(new IDrawable[]
        {
            new SurfaceType(material),
            new RenderFace(material),
            new AlphaClipping(),
            new GeometricSpecularAA()
        })
        {
        }

        public SurfaceOptions(IDrawable[] features) : base(features)
        {
            Label = SurfaceOptionsStyles.Label;
            Expandable = Expandable.SurfaceOptions;
        }
    }
}