using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class SurfaceInputs : СonstructedSection
    {
        private const string BaseOffsetPropertyName = "_BaseMap";

        public SurfaceInputs(IDrawable[] features) : base(features)
        {
            Label = SurfaceInputsStyles.Label;
            Expandable = Expandable.SurfaceInputs;
        }

        public SurfaceInputs() : this(new IDrawable[]
        {
            new BaseMap(),
            new TextureOffset(BaseOffsetPropertyName)
        })
        {
        }
    }
}