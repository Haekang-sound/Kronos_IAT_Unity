using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor
{
    public enum IridescenceModel
    {
        PhysicalBased,
        Approximation
    }

    public enum SheenModel
    {
        Approximation,
        PhysicalBased
    }
    
    internal static class URPPlusStyles
    {
        public static readonly GUIContent MaterialQualityOptions =
            EditorGUIUtility.TrTextContent("Materials");

        public static readonly GUIContent AmbientOcclusionOptions =
            EditorGUIUtility.TrTextContent("Ambient Occlusion");

        public static readonly GUIContent SSSModelText = 
            EditorGUIUtility.TrTextContent("SSS Model");
        
        public static readonly GUIContent IridescenceModelText = 
            EditorGUIUtility.TrTextContent("Iridescence Model");
        
        public static readonly GUIContent SheenModelText = 
            EditorGUIUtility.TrTextContent("Sheen Model");

        public static readonly GUIContent CoatSpecularAAText =
            EditorGUIUtility.TrTextContent("Coat Geometric Specular AA");

        public static readonly GUIContent ScreenFadeDistanceText =
            EditorGUIUtility.TrTextContent("Screen Fade Distance");

        public static readonly GUIContent MicroShadowsText = 
            EditorGUIUtility.TrTextContent("MicroShadows");
        
        public static readonly GUIContent MicroShadowsOpacityText = 
            EditorGUIUtility.TrTextContent("Opacity");

        public static readonly GUIContent HqDepthNormalsText =
            EditorGUIUtility.TrTextContent("High Quality DepthNormals");
    }
}