using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class AnisotropyStyles
    {
        public static readonly GUIContent TangentMap =
            EditorGUIUtility.TrTextContent("Tangent Map", "Specifies the Tangent Map for this Material.");

        public static readonly GUIContent Anisotropy =
            EditorGUIUtility.TrTextContent("Anisotropy", "Controls the scale factor for anisotropy.");

        public static readonly GUIContent AnisotropyMap =
            EditorGUIUtility.TrTextContent("Anisotropy Map", "Specifies the Anisotropy Map(R) for this Material.");
    }
}