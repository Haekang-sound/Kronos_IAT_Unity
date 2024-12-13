using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader
{
    public static class PropertyFinder
    {
        public static MaterialProperty FindOptionalProperty(string propertyName, MaterialProperty[] properties)
        {
            return FindProperty(propertyName, properties, false);
        }

        public static MaterialProperty FindProperty(string propertyName, MaterialProperty[] properties)
        {
            return FindProperty(propertyName, properties, true);
        }

        private static MaterialProperty FindProperty(string propertyName, IReadOnlyCollection<MaterialProperty> properties, bool propertyIsMandatory)
        {
            var property = properties.FirstOrDefault(prop => prop != null && prop.name == propertyName);
            if (property == null && propertyIsMandatory)
            {
                throw new ArgumentException($"Could not find MaterialProperty: '{propertyName}', Num properties: {properties.Count}");
            }
            return property;
        }
    }
}