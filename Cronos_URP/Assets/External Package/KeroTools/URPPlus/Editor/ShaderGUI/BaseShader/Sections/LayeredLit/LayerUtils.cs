using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.LayeredLit
{
    public class LayerUtils
    {
        private static readonly string[] Prefixes = { "", "1", "2", "3" };

        public static string LayerProperty(string keyWord, int layerIndex)
        {
            keyWord = $"{keyWord}{Prefixes[layerIndex]}";

            return keyWord;
        }

        public static MaterialProperty[] FindLayerProperty(string propertyName, MaterialProperty[] properties,
            uint layerCount = 2)
        {
            var arrayProperties = new MaterialProperty[layerCount];

            var prefixes = layerCount > 1 ? Prefixes : new[] { "" };

            for (var i = 0; i < layerCount; i++)
                arrayProperties[i] = PropertyFinder.FindOptionalProperty($"{propertyName}{prefixes[i]}", properties);

            return arrayProperties;
        }
    }
}