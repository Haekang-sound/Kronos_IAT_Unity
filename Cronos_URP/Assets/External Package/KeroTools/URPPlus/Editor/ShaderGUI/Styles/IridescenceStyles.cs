using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class IridescenceStyles
    {
        public static readonly GUIContent IridescenceLut = EditorGUIUtility.TrTextContent("Iridescence LUT");

        public static readonly GUIContent IridescenceMask = EditorGUIUtility.TrTextContent("Iridescence Mask",
            "Specifies the Iridescence Mask (R) for this Material - This map controls the intensity of the iridescence.");

        public static readonly GUIContent IridescenceThickness =
            EditorGUIUtility.TrTextContent("Iridescence Layer Thickness");

        public static readonly GUIContent IridescenceThicknessMap = EditorGUIUtility.TrTextContent(
            "Iridescence Layer Thickness map",
            "Specifies the Thickness map (R) of the thin iridescence layer over the material. Unit is micrometer multiplied by 3. A value of 1 is remapped to 3 micrometers or 3000 nanometers.");

        public static readonly GUIContent IridescenceThicknessRemap =
            EditorGUIUtility.TrTextContent("Iridescence Layer Thickness remap");
    }
}