using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class TransparencyStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Transparency Inputs");

        public static readonly GUIContent EnableRefraction = EditorGUIUtility.TrTextContent("Refraction");

        public static readonly GUIContent RefractionIor =
            EditorGUIUtility.TrTextContent("Index Of Refraction",
                "Controls the index of refraction for this Material.");

        public static readonly GUIContent EnableChromaticAberration =
            EditorGUIUtility.TrTextContent("Enable Chromatic Aberration");

        public static readonly GUIContent TransmittanceColor = EditorGUIUtility.TrTextContent("Transmittance Color",
            "Specifies the Transmittance Color (RGB) for this Material.");

        public static readonly GUIContent AtDistance = EditorGUIUtility.TrTextContent(
            "Transmittance Absorption Distance",
            "Sets the absorption distance reference in meters.");

        public static readonly GUIContent ChromaticAberration =
            EditorGUIUtility.TrTextContent("Chromatic Aberration",
                "Controls the chromatic aberration of refraction for this Material.");
        
        public static readonly GUIContent RefractionShadowAttenuation =
            EditorGUIUtility.TrTextContent("Shadow Attenuation",
                "Controls the shadow on refraction for this Material.");
    }
}