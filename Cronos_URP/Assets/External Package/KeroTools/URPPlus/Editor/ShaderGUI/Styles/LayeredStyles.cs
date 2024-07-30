using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class LayeredStyles
    {
        public static readonly GUIContent LayerMaskMap =
            EditorGUIUtility.TrTextContent("Layer Mask", "Specifies the Layer Mask for this Material");

        public static readonly GUIContent VertexColorMode = EditorGUIUtility.TrTextContent("Vertex Color Mode",
            "Specifies the method URP+ uses to color vertices.\nMultiply: Multiplies vertex color with the mask.\nAdditive: Remaps vertex color values between [-1, 1] and adds them to the mask (neutral value is 0.5 vertex color).");

        public static readonly GUIContent LayerCount =
            EditorGUIUtility.TrTextContent("Layer Count", "Controls the number of layers for this Material.");

        public static readonly GUIContent UseHeightBasedBlend =
            EditorGUIUtility.TrTextContent("Use Height Based Blend",
                "When enabled, URP+ blends the layer with the underlying layer based on the height.");

        public static readonly GUIContent UseMainLayerInfluenceMode =
            EditorGUIUtility.TrTextContent("Main Layer Influence",
                "Switches between regular layers mode and base/layers mode.");

        public static readonly GUIContent HeightTransition = EditorGUIUtility.TrTextContent("Height Transition",
            "Sets the size, in world units, of the smooth transition between layers.");

        public static readonly GUIContent LayerInfluenceMaskMap =
            EditorGUIUtility.TrTextContent("Layer Influence Mask",
                "Specifies the Layer Influence Mask for this Material.");

        public static readonly GUIContent OpacityAsDensity = EditorGUIUtility.TrTextContent(
            "Use Opacity map as Density map",
            "When enabled, URP+ uses the opacity map (alpha channel of Base Color) as the Density map.");

        public static readonly GUIContent InheritBaseNormal = EditorGUIUtility.TrTextContent("Normal influence",
            "Controls the strength of the normals inherited from the base layer.");

        public static readonly GUIContent InheritBaseHeight = EditorGUIUtility.TrTextContent("Heightmap influence",
            "Controls the strength of the height map inherited from the base layer.");

        public static readonly GUIContent InheritBaseColor = EditorGUIUtility.TrTextContent("BaseColor influence",
            "Controls the strength of the Base Color inherited from the base layer.");

        public static GUIContent[] Layers { get; } =
        {
            EditorGUIUtility.TrTextContent(" Main layer", Texture2D.whiteTexture),
            EditorGUIUtility.TrTextContent(" Layer 1", CoreEditorStyles.redTexture),
            EditorGUIUtility.TrTextContent(" Layer 2", CoreEditorStyles.greenTexture),
            EditorGUIUtility.TrTextContent(" Layer 3", CoreEditorStyles.blueTexture)
        };

        public static Expandable[] LayerExpandableBits { get; } =
        {
            Expandable.MainLayer,
            Expandable.Layer1,
            Expandable.Layer2,
            Expandable.Layer3
        };
    }
}