using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class TranslucencyStyles
    {
        public static readonly GUIContent TranslucencyScale =
            EditorGUIUtility.TrTextContent("Translucency Scale", "Controls the strength of the translucency.");

        public static readonly GUIContent TranslucencyPower =
            EditorGUIUtility.TrTextContent("Translucency Power", "Controls the power of the translucency.");

        public static readonly GUIContent TranslucencyAmbient =
            EditorGUIUtility.TrTextContent("Translucency Ambient", "Controls the ambient of the translucency.");

        public static readonly GUIContent TranslucencyDistortion =
            EditorGUIUtility.TrTextContent("Translucency Distortion", "Controls the distortion of the translucency.");

        public static readonly GUIContent TranslucencyShadows =
            EditorGUIUtility.TrTextContent("Translucency Shadows",
                "Controls the strength of the shadows that effects on translucency.");
    }
}