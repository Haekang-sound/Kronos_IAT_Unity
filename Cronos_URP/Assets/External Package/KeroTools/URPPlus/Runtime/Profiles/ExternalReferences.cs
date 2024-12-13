#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeroTools.URPPlus.Runtime.Profiles
{
    // This class only purpose is to be used as a sub-asset to a material and store references to other assets.
    // The goal is to be able to export the material as a package and not miss those referenced assets.
    public class ExternalReferences : ScriptableObject
    {
        [FormerlySerializedAs("DiffusionProfileReferences")] [SerializeField]
        private DiffusionProfileSettings[] _diffusionProfileReferences = Array.Empty<DiffusionProfileSettings>();
        
        [FormerlySerializedAs("WeatherProfileReferences")] [SerializeField]
        private WeatherProfileSettings[] _weatherProfileReferences = Array.Empty<WeatherProfileSettings>();

        public void SetDiffusionProfileReference(int index, DiffusionProfileSettings profile)
        {
            if (index >= _diffusionProfileReferences.Length)
            {
                var newList = new DiffusionProfileSettings[index + 1];
                for (var i = 0; i < _diffusionProfileReferences.Length; ++i)
                    newList[i] = _diffusionProfileReferences[i];

                _diffusionProfileReferences = newList;
            }

            _diffusionProfileReferences[index] = profile;
            EditorUtility.SetDirty(this);
        }
        
        public void SetWeatherProfileReference(int index, WeatherProfileSettings profile)
        {
            if (index >= _weatherProfileReferences.Length)
            {
                var newList = new WeatherProfileSettings[index + 1];
                for (var i = 0; i < _weatherProfileReferences.Length; ++i)
                    newList[i] = _weatherProfileReferences[i];

                _weatherProfileReferences = newList;
            }

            _weatherProfileReferences[index] = profile;
            EditorUtility.SetDirty(this);
        }

        public static ExternalReferences GetMaterialExternalReferences(Material material)
        {
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(material));
            var matExternalRefs =
                (from subAsset in subAssets
                    where subAsset.GetType() == typeof(ExternalReferences)
                    select subAsset as ExternalReferences).FirstOrDefault();

            if (matExternalRefs != null) 
                return matExternalRefs;
            
            matExternalRefs = CreateInstance<ExternalReferences>();
            matExternalRefs.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
            AssetDatabase.AddObjectToAsset(matExternalRefs, material);
            EditorUtility.SetDirty(matExternalRefs);
            EditorUtility.SetDirty(material);

            return matExternalRefs;
        }
    }
}
#endif