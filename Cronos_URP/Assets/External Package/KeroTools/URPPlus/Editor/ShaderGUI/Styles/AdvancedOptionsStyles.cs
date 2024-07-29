using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class AdvancedOptionsStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Advanced Options",
            "These settings affect behind-the-scenes rendering and underlying calculations.");
        
        public static readonly GUIContent CastShadow = EditorGUIUtility.TrTextContent("Cast Shadows",
            "When enabled, this GameObject can cast shadows onto other GameObjects.");

        public static readonly GUIContent ReceiveShadow = EditorGUIUtility.TrTextContent("Receive Shadows",
            "When enabled, other GameObjects can cast shadows onto this GameObject.");

        public static readonly GUIContent Highlights = EditorGUIUtility.TrTextContent("Specular Highlights",
            "When enabled, the Material reflects the shine from direct lighting.");

        public static readonly GUIContent Reflections = EditorGUIUtility.TrTextContent("Environment Reflections",
            "When enabled, the Material samples reflections from the nearest Reflection Probes or Lighting Probe.");

        public static readonly GUIContent SecondaryClearCoatNormal =
            EditorGUIUtility.TrTextContent("ClearCoat Second Normal",
                "When enabled, the Material calculate another normal for ClearCoat.");

        public static readonly GUIContent HorizonOcclusion = EditorGUIUtility.TrTextContent("Horizon Occlusion");
        public static readonly GUIContent HorizonFade = EditorGUIUtility.TrTextContent("Horizon Fade");

        public static readonly GUIContent SpecularOcclusionMode =
            EditorGUIUtility.TrTextContent("Specular Occlusion Mode",
                "Determines the mode used to compute specular occlusion");

        public static readonly GUIContent GIOcclusionBias =
            EditorGUIUtility.TrTextContent("GIOcclusion Bias", "Controls bias of specular occlusion from GI");

        public static readonly GUIContent QueueSlider = EditorGUIUtility.TrTextContent("Sorting Priority",
            "Determines the chronological rendering order for a Material. Materials with lower value are rendered first.");

        public static readonly GUIContent QueueControl = EditorGUIUtility.TrTextContent("Queue Control",
            "Controls whether render queue is automatically set based on material surface type, or explicitly set by the user.");
    }
}