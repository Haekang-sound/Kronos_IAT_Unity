using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class HairStyles
    {
        public static readonly GUIContent SmoothnessMask = EditorGUIUtility.TrTextContent("Smoothness Mask",
            "Assign a Texture that defines the smoothness for this material.");

        public static readonly GUIContent SpecularColorHair = EditorGUIUtility.TrTextContent("Specular Color",
            "Set the representative color of the highlight that Unity uses to drive both the primary specular highlight color, which is mainly monochrome, and the secondary specular highlight color, which is chromatic.");

        public static readonly GUIContent SpecularMultiplier = EditorGUIUtility.TrTextContent("Specular Multiplier",
            "Modifies the primary specular highlight by this multiplier.");

        public static readonly GUIContent SpecularShift = EditorGUIUtility.TrTextContent("Specular Shift",
            "Modifies the position of the primary specular highlight.");

        public static readonly GUIContent SecondarySpecularMultiplier =
            EditorGUIUtility.TrTextContent("Secondary Specular Multiplier",
                "Modifies the secondary specular highlight by this multiplier.");

        public static readonly GUIContent SecondarySpecularShift =
            EditorGUIUtility.TrTextContent("Secondary Specular Shift",
                "Modifies the position of the secondary specular highlight.");

        public static readonly GUIContent TransmissionColor = EditorGUIUtility.TrTextContent("Transmission Color",
            "Set the fraction of specular lighting that penetrates the hair from behind.");

        public static readonly GUIContent TransmissionRim = EditorGUIUtility.TrTextContent("Transmission Rim",
            "Set the intensity of back lit hair around the edge of the hair.");

        public static readonly GUIContent StaticLightColor =
            EditorGUIUtility.TrTextContent("Static Light Color", "Specifies the static light color of the Material.");

        public static readonly GUIContent StaticLightVector = EditorGUIUtility.TrTextContent("Static Light Vector",
            "Specifies the static light vector of the Material. XYZ - direction, W - intensity.");

        public static readonly GUIContent StaticHighlight = EditorGUIUtility.TrTextContent(
            "Static Specular Highlight",
            "When enabled, the Material reflects the shine from vector direction in material properties.");
    }
}