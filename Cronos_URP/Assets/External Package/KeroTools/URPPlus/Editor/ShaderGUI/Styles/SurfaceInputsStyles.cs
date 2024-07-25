using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    public static class SurfaceInputsStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Surface Inputs",
            "These settings describe the look and feel of the surface itself.");
        
        public static readonly GUIContent BaseMap = EditorGUIUtility.TrTextContent("Base Map",
            "Specifies the base color (RGB) and opacity (A) of the Material.");

        public static readonly GUIContent AlphaRemapping = EditorGUIUtility.TrTextContent("Alpha Remapping",
            "Controls a remap for the alpha channel in the Base Map.");

        public static readonly GUIContent Metallic = EditorGUIUtility.TrTextContent("Metallic",
            "Controls the scale factor for the Material's metallic effect.");

        public static readonly GUIContent MetallicRemapping = EditorGUIUtility.TrTextContent("Metallic Remapping",
            "Controls a remap for the metallic channel in the Mask Map.");

        public static readonly GUIContent Smoothness =
            EditorGUIUtility.TrTextContent("Smoothness", "Controls the scale factor for the Material's smoothness.");

        public static readonly GUIContent SmoothnessRemapping =
            EditorGUIUtility.TrTextContent("Smoothness Remapping",
                "Controls a remap for the smoothness channel in the Mask Map.");

        public static readonly GUIContent AORemapping = EditorGUIUtility.TrTextContent(
            "Ambient Occlusion Remapping", "Controls a remap for the ambient occlusion channel in the Mask Map.");

        public static readonly GUIContent MaskMap = EditorGUIUtility.TrTextContent("Mask Map",
            "Specifies the Mask Map for this Material - Metallic (R), Ambient occlusion (G), Detail mask (B), Smoothness (A).");
        
        public static readonly GUIContent AOMap = EditorGUIUtility.TrTextContent("AO Map",
            "Assign a Texture that defines the ambient occlusion for this material.");

        public static readonly GUIContent MaskMapSpecular = EditorGUIUtility.TrTextContent("Mask Map",
            "Specifies the Mask Map for this Material - Ambient occlusion (G), Detail mask (B), Smoothness (A).");

        public static readonly GUIContent SpecularColor =
            EditorGUIUtility.TrTextContent("Specular Color", "Specifies the Specular color (RGB) of this Material.");

        public static readonly GUIContent NormalMap = EditorGUIUtility.TrTextContent("Normal Map",
            "Specifies the Normal Map for this Material and controls its strength.");
        
        public static readonly GUIContent CoatNormalMap = EditorGUIUtility.TrTextContent("Coat normal map",
            "Specifies the ClearCoat Normal Map for this Material and controls its strength.");

        public static readonly GUIContent BentNormalMap = EditorGUIUtility.TrTextContent("Bent normal map",
            "Specifies the cosine weighted Bent Normal Map for this Material. Use only with indirect diffuse lighting (Lightmaps and Light Probes).");

        public static readonly GUIContent FixNormal =
            EditorGUIUtility.TrTextContent("Fix now", "Converts the assigned texture to be a normal map format.");

        public static readonly GUIContent BumpScaleNotSupported =
            EditorGUIUtility.TrTextContent("Bump scale is not supported on mobile platforms");
    }
}