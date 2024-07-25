using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class HeightBlockStyles
    {
        public static readonly GUIContent HeightMap = EditorGUIUtility.TrTextContent("Height Map",
            "Specifies the Height Map (R) for this Material.\nFor floating point textures, set the Min, Max, and base values to 0, 1, and 0 respectively.");

        public static readonly GUIContent HeightMapCenter =
            EditorGUIUtility.TrTextContent("Base", "Controls the base of the Height Map (between 0 and 1).");

        public static readonly GUIContent HeightMapMin =
            EditorGUIUtility.TrTextContent("Min", "Sets the minimum value in the Height Map (in centimeters).");

        public static readonly GUIContent HeightMapMax =
            EditorGUIUtility.TrTextContent("Max", "Sets the maximum value in the Height Map (in centimeters).");

        public static readonly GUIContent HeightMapAmplitude =
            EditorGUIUtility.TrTextContent("Amplitude", "Sets the amplitude of the Height Map (in centimeters).");

        public static readonly GUIContent HeightMapOffset =
            EditorGUIUtility.TrTextContent("Offset",
                "Sets the offset URP+ applies to the Height Map (in centimeters).");

        public static readonly GUIContent HeightMapParametrization =
            EditorGUIUtility.TrTextContent("Parametrization",
                "Specifies the parametrization method for the Height Map.");
    }
}