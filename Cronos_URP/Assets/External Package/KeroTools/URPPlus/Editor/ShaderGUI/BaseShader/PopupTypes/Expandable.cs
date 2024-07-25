using System;

namespace KeroTools.URPPlus.Editor.ShaderGUI
{
    [Flags]
    public enum Expandable
    {
        SurfaceOptions = 1 << 0,
        TessellationOptions = 1 << 1,
        SurfaceInputs = 1 << 2,
        MainLayer = 1 << 3,
        Layer1 = 1 << 4,
        Layer2 = 1 << 5,
        Layer3 = 1 << 6,
        WeatherInputs = 1 << 7,
        TransparencyInputs = 1 << 8,
        DetailInputs = 1 << 9,
        EmissionInputs = 1 << 10,
        AdvancedOptions = 1 << 11
    }
}