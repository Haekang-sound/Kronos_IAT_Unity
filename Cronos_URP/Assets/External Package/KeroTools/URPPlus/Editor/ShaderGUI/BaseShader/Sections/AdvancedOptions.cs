using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.AdvancedOptionsFeatures;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections
{
    public class AdvancedOptions : СonstructedSection
    {
        private readonly IDrawable[] _features;

        public AdvancedOptions() : this(new IDrawable[]
        {
            new ReceiveShadows(),
            new HighlightReflections(),
            new QueueController(),
            new GPUInstancing()
        })
        {
        }

        public AdvancedOptions(IDrawable[] features) : base(features)
        {
            Label = AdvancedOptionsStyles.Label;
            Expandable = Expandable.AdvancedOptions;
        }
    }
}