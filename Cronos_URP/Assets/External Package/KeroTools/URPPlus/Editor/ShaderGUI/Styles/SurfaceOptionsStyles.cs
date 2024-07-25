using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class SurfaceOptionsStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Surface Options",
                "Controls the rendering states of the fullscreen material.");
        
        public static readonly GUIContent WorkflowMode = EditorGUIUtility.TrTextContent("Workflow Mode",
            "Select a workflow that fits your textures. Choose between Metallic or Specular.");

        public static readonly GUIContent SurfaceType = EditorGUIUtility.TrTextContent("Surface Type",
            "Controls whether the Material supports transparency or not");

        public static readonly GUIContent BlendingMode = EditorGUIUtility.TrTextContent("Blending Mode",
            "Controls how the color of the Transparent surface blends with the Material color in the background.");

        public static readonly GUIContent PreserveSpecular = EditorGUIUtility.TrTextContent(
            "Preserve Specular Lighting",
            "Preserves specular lighting intensity and size by not applying transparent alpha to the specular light contribution.");

        public static readonly GUIContent Culling = EditorGUIUtility.TrTextContent("Render Face",
            "Specifies which faces to cull from your geometry. Front culls front faces. Back culls backfaces. None means that both sides are rendered.");

        public static readonly GUIContent DoubleSidedNormalMode = EditorGUIUtility.TrTextContent("Normal Mode",
            "Specifies the method URP+ uses to modify the normal base.\nMirror: Mirrors the normals with the vertex normal plane.\nFlip: Flips the normal.");

        public static readonly GUIContent AlphaCutoffEnable = EditorGUIUtility.TrTextContent("Alpha Clipping",
            "When enabled, URP+ processes Alpha Clipping for this Material.");

        public static readonly GUIContent AlphaCutoff =
            EditorGUIUtility.TrTextContent("Threshold", "Controls the threshold for the Alpha Clipping effect.");

        public static readonly GUIContent UseShadowThreshold =
            EditorGUIUtility.TrTextContent("Use Shadow Threshold", "Enable separate threshold for shadow pass");

        public static readonly GUIContent AlphaCutoffShadow =
            EditorGUIUtility.TrTextContent("Shadow Threshold",
                "Controls the threshold for shadow pass alpha clipping.");

        public static readonly GUIContent AlphaToMask = EditorGUIUtility.TrTextContent("Alpha To Mask",
            "When enabled and using MSAA, URP+ enables alpha to coverage during the depth prepass.");

        public static readonly GUIContent MaterialID = EditorGUIUtility.TrTextContent("Material Type",
            "Specifies additional feature for this Material. Customize you Material with different settings depending on which Material Type you select.");

        public static readonly GUIContent EnableTransmission = EditorGUIUtility.TrTextContent("Enable Transmission",
            "When enabled URP+ processes the transmission effect for subsurface scattering. Simulates the translucency of the object.");

        public static readonly GUIContent ZWriteEnable = EditorGUIUtility.TrTextContent("Depth Write",
            "When enabled, transparent objects write to the depth buffer.");

        public static readonly GUIContent TransparentZTest =
            EditorGUIUtility.TrTextContent("Depth Test", "Set the comparison function to use during the Z Testing.");

        public static readonly GUIContent DisplacementMode = EditorGUIUtility.TrTextContent("Displacement Mode",
            "Specifies the method URP+ uses to apply height map displacement to the selected element: Vertex, pixel, or tessellated vertex.\nYou must use flat surfaces for Pixel displacement.");

        public static readonly GUIContent LockWithObjectScale = EditorGUIUtility.TrTextContent(
            "Lock With Object Scale",
            "When enabled, displacement mapping takes the absolute value of the scale of the object into account.");

        public static readonly GUIContent LockWithTilingRate = EditorGUIUtility.TrTextContent(
            "Lock With Height Map Tiling Rate",
            "When enabled, displacement mapping takes the absolute value of the tiling rate of the height map into account.");

        public static readonly GUIContent GeometricSpecularAA =
            EditorGUIUtility.TrTextContent("Geometric Specular AA",
                "When enabled, URP+ reduces specular aliasing on high density meshes (particularly useful when the not using a normal map).");

        public static readonly GUIContent SpecularAAScreenSpaceVariance =
            EditorGUIUtility.TrTextContent("Screen space variance",
                "Controls the strength of the Specular AA reduction. Higher values give a more blurry result and less aliasing.");

        public static readonly GUIContent SpecularAAThreshold = EditorGUIUtility.TrTextContent("Threshold",
            "Controls the effect of Specular AA reduction. A values of 0 does not apply reduction, higher values allow higher reduction.");
    }
}