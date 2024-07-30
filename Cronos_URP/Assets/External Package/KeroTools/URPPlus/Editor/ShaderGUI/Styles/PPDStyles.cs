using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    // ReSharper disable once InconsistentNaming
    internal static class PPDStyles
    {
        public static readonly GUIContent PpdMinSamples = EditorGUIUtility.TrTextContent("Minimum Steps",
            "Controls the minimum number of steps URP+ uses for per pixel displacement mapping.");

        public static readonly GUIContent PpdMaxSamples = EditorGUIUtility.TrTextContent("Maximum Steps",
            "Controls the maximum number of steps URP+ uses for per pixel displacement mapping.");

        public static readonly GUIContent PpdLodThreshold = EditorGUIUtility.TrTextContent("Fading Mip Level Start",
            "Controls the Height Map mip level where the parallax occlusion mapping effect begins to disappear.");

        public static readonly GUIContent PpdPrimitiveLength = EditorGUIUtility.TrTextContent("Primitive Length",
            "Sets the length of the primitive (with the scale of 1) to which URP+ applies per-pixel displacement mapping. For example, the standard quad is 1 x 1 meter, while the standard plane is 10 x 10 meters.");

        public static readonly GUIContent PpdPrimitiveWidth = EditorGUIUtility.TrTextContent("Primitive Width",
            "Sets the width of the primitive (with the scale of 1) to which URP+ applies per-pixel displacement mapping. For example, the standard quad is 1 x 1 meter, while the standard plane is 10 x 10 meters.");
    }
}