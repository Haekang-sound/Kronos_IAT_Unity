using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class SubSurfaceScatteringStyles
    {
        public static readonly GUIContent DiffusionProfileLabel = EditorGUIUtility.TrTextContent("Diffusion Profile");
        
        public static readonly GUIContent Thickness = EditorGUIUtility.TrTextContent("Thickness",
            "Controls the strength of the Thickness Map, low values allow some light to transmit through the object.");

        public static readonly GUIContent Curvature =
            EditorGUIUtility.TrTextContent("Curvature", "Controls the strength of the Curvature Map.");

        public static readonly GUIContent ThicknessMap = EditorGUIUtility.TrTextContent("Thickness Map",
            "Specifies the Thickness Map (R) for this Material - This map describes the thickness of the object. When subsurface scattering is enabled, low values allow some light to transmit through the object.");

        public static readonly GUIContent ThicknessCurvatureMap = EditorGUIUtility.TrTextContent("TC Map",
            "Specifies the Thickness(R) and Curvature(G) for this Material - This map describes the thickness and curvature of the object. When subsurface scattering or translucency is enabled, low values allow some light to transmit through the object.");

        public static readonly GUIContent ThicknessRemap = EditorGUIUtility.TrTextContent("Thickness Remapping",
            "Controls a remap for the Thickness Map from [0, 1] to the specified range.");

        public static readonly GUIContent CurvatureRemap = EditorGUIUtility.TrTextContent("Curvature Remapping",
            "Controls a remap for the Curvature Map from [0, 1] to the specified range.");

        public static readonly GUIContent TransmissionScale =
            EditorGUIUtility.TrTextContent("Transmission Scale", "Controls the strength of the transmission.");
    }
}