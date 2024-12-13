using System;
using JetBrains.Annotations;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Runtime;
using KeroTools.URPPlus.Runtime.Profiles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class WeatherProfile : IDrawable
    {
        private readonly Material _material;

        protected MaterialProperty WeatherProfileAssetProperty;
        protected MaterialProperty WeatherProfileHashProperty;
        public WeatherProfile(Material material) => 
            _material = material;

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            WeatherProfileAssetProperty = GetProperty("_WeatherProfileAsset");
            WeatherProfileHashProperty = GetProperty("_WeatherProfileHash");
            return;

            MaterialProperty GetProperty(string propertyName) =>
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor) => 
            DrawAssetField(editor, WeatherInputsStyles.WeatherProfileLabel, WeatherProfileAssetProperty, WeatherProfileHashProperty);

        public void SetKeywords(Material material) { }
        
        protected virtual void DrawAssetField(PropertiesEditor editor, GUIContent label, MaterialProperty profileAsset,
            MaterialProperty profileHash)
        {
            var guid = RPUtils.ConvertVector4ToGuid(profileAsset.vectorValue);
            var profileSettings = GetProfileSettingsFromGuid(guid);

            EditorGUI.BeginChangeCheck();
            profileSettings = (WeatherProfileSettings)EditorGUILayout.ObjectField(label, profileSettings,
                typeof(WeatherProfileSettings), false);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateProfileSettings(profileSettings, profileAsset, profileHash, guid);
                UpdateExternalReference(editor, profileSettings);
            }
        }
        
        private WeatherProfileSettings GetProfileSettingsFromGuid(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            return string.IsNullOrEmpty(assetPath) ? null : AssetDatabase.LoadAssetAtPath<WeatherProfileSettings>(assetPath);
        }

        private void UpdateProfileSettings(WeatherProfileSettings profileSettings, MaterialProperty profileAsset, 
            MaterialProperty profileHash, string guid)
        {
            if (guid == null) 
                throw new ArgumentNullException(nameof(guid));
            
            var newGuid = Vector4.zero;
            float hash = 0;

            if (profileSettings != null)
            {
                guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(profileSettings));
                newGuid = RPUtils.ConvertGuidToVector4(guid);
                hash = RPUtils.AsFloat(profileSettings._hash);

                profileSettings.SetMaterialProfile(_material);
                profileSettings.AddChildMaterial(_material);
            }

            if (profileAsset.vectorValue != newGuid)
            {
                var oldProfileGuid = RPUtils.ConvertVector4ToGuid(profileAsset.vectorValue);
                var oldProfileSettings =
                    AssetDatabase.LoadAssetAtPath<WeatherProfileSettings>(AssetDatabase.GUIDToAssetPath(oldProfileGuid));

                if (oldProfileSettings != null && oldProfileSettings != profileSettings)
                    oldProfileSettings.RemoveChildMaterial(_material);
            }

            profileAsset.vectorValue = newGuid;
            profileHash.floatValue = hash;
        }

        private void UpdateExternalReference(PropertiesEditor editor, WeatherProfileSettings profileSettings)
        {
            foreach (var target in editor.GetTargets())
            {
                var matExternalRefs = ExternalReferences.GetMaterialExternalReferences(target as Material);
                matExternalRefs.SetWeatherProfileReference(0, profileSettings);
            }
        }
    }
}