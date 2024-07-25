using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Styles
{
    internal static class TessellationStyles
    {
        public static readonly GUIContent Label = EditorGUIUtility.TrTextContent("Tessellation Options");

        public static readonly GUIContent Mode = EditorGUIUtility.TrTextContent("Tessellation Mode", 
            "Controls mode of tessellation factor");

        public static readonly GUIContent PhongTessellation = EditorGUIUtility.TrTextContent("Phong Tessellation",
            "Phong tessellation applies vertex interpolation to make geometry smoother. If you assign a displacement map for this Material and select this option, URP+ applies smoothing to the displacement map.");

        public static readonly GUIContent Factor = EditorGUIUtility.TrTextContent("Tessellation Factor",
            "Controls the strength of the tessellation effect. Higher values result in more tessellation. Maximum tessellation factor is 15 on the Xbox One and PS4");

        public static readonly GUIContent FactorMinDistance = EditorGUIUtility.TrTextContent("Start Fade Distance",
                "Sets the distance from the camera at which tessellation begins to fade out.");

        public static readonly GUIContent FactorMaxDistance = EditorGUIUtility.TrTextContent("End Fade Distance",
                "Sets the maximum distance from the Camera where URP+ tessellates triangle. Set to 0 to disable adaptative factor with distance.");

        public static readonly GUIContent FactorTriangleSize = EditorGUIUtility.TrTextContent("Triangle Size",
            "Sets the desired screen space size of triangles (in pixels). Smaller values result in smaller triangle. Set to 0 to disable adaptative factor with screen space size.");

        public static readonly GUIContent ShapeFactor = EditorGUIUtility.TrTextContent("Shape Factor",
            "Controls the strength of Phong tessellation shape (lerp factor).");

        public static readonly GUIContent BackFaceCullEpsilon = EditorGUIUtility.TrTextContent("Triangle Culling Epsilon",
            "Controls triangle culling. A value of -1.0 disables back face culling for tessellation, higher values produce more aggressive culling and better performance.");

        public static readonly GUIContent MaxDisplacement = EditorGUIUtility.TrTextContent("Max Displacement",
            "Positive maximum displacement in meters of the current displaced geometry. This is used to adapt the culling algorithm in case of large deformation. It can be the maximum height in meters of a heightmap for example.");
    }
}