using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class DetailInputsStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Detail Inputs");
        
        public static readonly GUIContent DetailMap = EditorGUIUtility.TrTextContent("Detail Map",
            "Specifies the Detail Map albedo (R) Normal map y-axis (G) Smoothness (B) Normal map x-axis (A) - Neutral value is (0.5, 0.5, 0.5, 0.5)");

        public static readonly GUIContent DetailAlbedoScale = EditorGUIUtility.TrTextContent("Detail Albedo Scale",
            "Controls the scale factor for the Detail Map's Albedo.");

        public static readonly GUIContent DetailNormalScale = EditorGUIUtility.TrTextContent("Detail Normal Scale",
            "Controls the scale factor for the Detail Map's Normal map.");

        public static readonly GUIContent DetailSmoothnessScale =
            EditorGUIUtility.TrTextContent("Detail Smoothness Scale",
                "Controls the scale factor for the Detail Map's Smoothness.");
    }
}