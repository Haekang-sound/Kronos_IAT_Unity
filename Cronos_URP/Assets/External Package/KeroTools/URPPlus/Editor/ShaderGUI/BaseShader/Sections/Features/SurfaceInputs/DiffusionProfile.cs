using System;
using KeroTools.URPPlus.Editor.ShaderGUI.Editor;
using KeroTools.URPPlus.Editor.ShaderGUI.Styles;
using KeroTools.URPPlus.Runtime;
using KeroTools.URPPlus.Runtime.Profiles;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.ShaderGUI.BaseShader.Sections.Features.SurfaceInputs
{
    public class DiffusionProfile : IDrawable
    {
        private static readonly int MaterialTypeId = Shader.PropertyToID("_MaterialType");

        private readonly MaterialTypeMode? _overrideMode;
        private readonly Material _material;

        protected MaterialProperty DiffusionProfileAssetProperty;
        protected MaterialProperty DiffusionProfileHashProperty;

        public DiffusionProfile(Material material, MaterialTypeMode? overrideMode = null)
        {
            _material = material;
            _overrideMode = overrideMode;
        }

        public virtual void FindProperties(MaterialProperty[] properties)
        {
            DiffusionProfileAssetProperty = GetProperty("_DiffusionProfileAsset");
            DiffusionProfileHashProperty = GetProperty("_DiffusionProfileHash");
            return;

            MaterialProperty GetProperty(string propertyName) => 
                PropertyFinder.FindOptionalProperty(propertyName, properties);
        }

        public virtual void Draw(PropertiesEditor editor)
        {
            var currentMaterialType = _overrideMode ?? GetMaterialType();

            if (currentMaterialType is MaterialTypeMode.SubSurfaceScattering or MaterialTypeMode.Translucency)
                DrawAssetField(editor, SubSurfaceScatteringStyles.DiffusionProfileLabel, DiffusionProfileAssetProperty, DiffusionProfileHashProperty);
        }

        public void SetKeywords(Material material) { }
        
        protected virtual void DrawAssetField(PropertiesEditor editor, GUIContent label, MaterialProperty profileAsset,
            MaterialProperty profileHash)
        {
            var guid = RPUtils.ConvertVector4ToGuid(profileAsset.vectorValue);
            var profileSettings = GetProfileSettingsFromGuid(guid);

            EditorGUI.BeginChangeCheck();
            profileSettings = (DiffusionProfileSettings)EditorGUILayout.ObjectField(label, profileSettings,
                typeof(DiffusionProfileSettings), false);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateProfileSettings(profileSettings, profileAsset, profileHash, guid);
                UpdateExternalReference(editor, profileSettings);
            }
        }
        
        private DiffusionProfileSettings GetProfileSettingsFromGuid(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            return string.IsNullOrEmpty(assetPath) ? null : AssetDatabase.LoadAssetAtPath<DiffusionProfileSettings>(assetPath);
        }

        private void UpdateProfileSettings(DiffusionProfileSettings profileSettings, MaterialProperty profileAsset, 
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
                    AssetDatabase.LoadAssetAtPath<DiffusionProfileSettings>(
                        AssetDatabase.GUIDToAssetPath(oldProfileGuid));

                if (oldProfileSettings != null && oldProfileSettings != profileSettings)
                    oldProfileSettings.RemoveChildMaterial(_material);
            }

            profileAsset.vectorValue = newGuid;
            profileHash.floatValue = hash;
        }

        private void UpdateExternalReference(PropertiesEditor editor, DiffusionProfileSettings profileSettings)
        {
            foreach (var target in editor.GetTargets())
            {
                var matExternalRefs = ExternalReferences.GetMaterialExternalReferences(target as Material);
                matExternalRefs.SetDiffusionProfileReference(0, profileSettings);
            }
        }

        private MaterialTypeMode GetMaterialType()
        {
            if (_material.HasProperty(MaterialTypeId))
                return (MaterialTypeMode)_material.GetFloat(MaterialTypeId);

            return MaterialTypeMode.Standard;
        }
    }
}