using UnityEngine;

namespace KeroTools.URPPlus.Runtime
{
    public static class GlobalVariables
    {
        public const string PreIntegratedSssKeyword = "_SHADER_QUALITY_PREINTEGRATED_SSS";
        public const string PreIntegratedIridescenceKeyword = "_SHADER_QUALITY_IRIDESCENCE_APPROXIMATION";
        public const string PbSheenKeyword = "_SHADER_QUALITY_SHEEN_PHYSICAL_BASED";
        public const string CoatSpecularAAKeyword = "_SHADER_QUALITY_COAT_GEOMETRIC_AA";
        public const string MicroShadowsKeyword = "_SHADER_QUALITY_MICRO_SHADOWS";
        public const string HqDepthNormalsKeyword = "_SHADER_QUALITY_HIGH_QUALITY_DEPTH_NORMALS";

        public static readonly int SsRefractionInvScreenWeightDistance =
            Shader.PropertyToID("_SSRefractionInvScreenWeightDistance");

        public static readonly int MicroShadowOpacity = Shader.PropertyToID("_MicroShadowOpacity");
        
        public static readonly int RainMultiplier = Shader.PropertyToID("_RainMultiplier");
        public static readonly int Wetness = Shader.PropertyToID("_Wetness");
        public static readonly int WetnessColor = Shader.PropertyToID("_WetnessColor");
    }
}