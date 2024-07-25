using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class ClearCoatStyles
    {
        public static readonly GUIContent ClearCoatMask = EditorGUIUtility.TrTextContent("Coat Mask",
            "Specifies the amount of the coat blending." +
            "\nActs as a multiplier of the clear coat map mask value or as a direct mask value if no map is specified." +
            "\nThe map specifies clear coat mask in the red channel and clear coat smoothness in the green channel.");

        public static readonly GUIContent ClearCoatSmoothness = EditorGUIUtility.TrTextContent(
            "ClearCoat Smoothness",
            "Specifies the smoothness of the coating." +
            "\nActs as a multiplier of the clear coat map smoothness value or as a direct smoothness value if no map is specified.");

        public static readonly GUIContent CoatNormalMap = EditorGUIUtility.TrTextContent("Coat Normal Map",
            "Specifies the Coat Normal Map for this Material and controls its strength.");
    }
}