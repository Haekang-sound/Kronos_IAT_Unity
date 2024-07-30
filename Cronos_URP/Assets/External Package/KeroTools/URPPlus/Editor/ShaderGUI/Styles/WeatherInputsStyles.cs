using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    public static class WeatherInputsStyles
    {
        public static readonly GUIContent WeatherProfileLabel = EditorGUIUtility.TrTextContent("Weather Profile");
        
        public static readonly GUIContent WeatherEnable = EditorGUIUtility.TrTextContent("Weather Enable");
        public static readonly GUIContent RainMode = EditorGUIUtility.TrTextContent("Rain Mode");
        
        public static readonly GUIContent PuddlesNormalIntensity = EditorGUIUtility.TrTextContent("Puddles Normal Intensity");
        public static readonly GUIContent RainNormalIntensity = EditorGUIUtility.TrTextContent("Rain Normal Intensity");
        
        public static readonly GUIContent RainDistortion = EditorGUIUtility.TrTextContent("Rain Distortion");
        public static readonly GUIContent RainDistortionSize = EditorGUIUtility.TrTextContent("Rain Distortion Size");
        public static readonly GUIContent RainMask = EditorGUIUtility.TrTextContent("Rain Mask");
        public static readonly GUIContent RainWetnessFactor = EditorGUIUtility.TrTextContent("Rain Wetness Factor");
    }
}