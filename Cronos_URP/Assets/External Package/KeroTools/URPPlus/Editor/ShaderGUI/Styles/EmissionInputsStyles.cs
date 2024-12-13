using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class EmissionInputsStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Emission Inputs");

        public static readonly GUIContent EmissiveMap =
            EditorGUIUtility.TrTextContent("Emissive Map", "Specifies the emissive color (RGB) of the Material.");

        public static readonly GUIContent EmissiveIntensity =
            EditorGUIUtility.TrTextContent("Emission Intensity", "Emission intensity in scale factor");

        public static readonly GUIContent AlbedoAffectEmissive =
            EditorGUIUtility.TrTextContent("Emission multiply with Base",
                "Specifies whether or not the emission color is multiplied by the albedo.");
        
        public static readonly GUIContent EmissionFresnel =
            EditorGUIUtility.TrTextContent("Emission Fresnel",
                "When enabled, the Material multiply emission with fresnel.");
        
        public static readonly GUIContent EmissionFresnelPower =
            EditorGUIUtility.TrTextContent("Emission Fresnel Power");
    }
}