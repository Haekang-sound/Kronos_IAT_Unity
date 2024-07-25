using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeroTools.URPPlus.Runtime.Profiles
{
    [CreateAssetMenu(fileName = "Weather Profile", menuName = "Rendering/URP+/Weather Profile")]
    public class WeatherProfileSettings : ScriptableObject, IProfile
    {
        [FormerlySerializedAs("puddleNormal")] 
        public Texture2D _puddleNormal;
        [FormerlySerializedAs("puddlesFramesSize")]
        public Vector2 _puddlesFramesSize;
        [FormerlySerializedAs("puddlesSize")]
        public float _puddlesSize;
        [FormerlySerializedAs("puddlesAnimationSpeed")]
        public float _puddlesAnimationSpeed;
        
        [FormerlySerializedAs("rainNormal")] 
        public Texture2D _rainNormal;
        [FormerlySerializedAs("rainSize")]
        public float _rainSize;
        [FormerlySerializedAs("rainAnimationSpeed")]
        public float _rainAnimationSpeed;
        
        [SerializeField] private List<Material> _childMaterials;
        
        [FormerlySerializedAs("hash")] public uint _hash;
        
        private static readonly int PuddlesNormal = Shader.PropertyToID("_PuddlesNormal");
        private static readonly int PuddlesFramesSize = Shader.PropertyToID("_PuddlesFramesSize");
        private static readonly int PuddlesSize = Shader.PropertyToID("_PuddlesSize");
        private static readonly int PuddlesAnimationSpeed = Shader.PropertyToID("_PuddlesAnimationSpeed");
        
        private static readonly int RainNormal = Shader.PropertyToID("_RainNormal");
        private static readonly int RainSize = Shader.PropertyToID("_RainSize");
        private static readonly int RainAnimationSpeed = Shader.PropertyToID("_RainAnimationSpeed");
        
        public WeatherProfileSettings() => 
            ResetToDefault();
        
        private void OnValidate() => 
            SetMaterialsProfile();
        
        public void ResetToDefault()
        {
            ResetPuddlesSettings();
            ResetRainSettings();

            SetMaterialsProfile();
        }

        private void ResetPuddlesSettings()
        {
            _puddleNormal = null;
            _puddlesFramesSize = Vector2.zero;
            _puddlesSize = 1.0f;
            _puddlesAnimationSpeed = 0.1f;
        }

        private void ResetRainSettings()
        {
            _rainNormal = null;
            _rainSize = 1.0f;
            _rainAnimationSpeed = 0.1f;
        }

        public void SetMaterialsProfile()
        {
            if (_childMaterials is null) 
                return;
            
            foreach (var material in _childMaterials)
                SetMaterialProfile(material);
        }
        
        public void SetMaterialProfile(Material material)
        {
            SetPuddlesSettings(material);
            SetRainSettings(material);
        }

        private void SetRainSettings(Material material)
        {
            if (material.HasProperty(RainNormal))
                material.SetTexture(RainNormal, _rainNormal);
            
            if (material.HasProperty(RainSize))
                material.SetFloat(RainSize, _rainSize);
            
            if (material.HasProperty(RainAnimationSpeed))
                material.SetFloat(RainAnimationSpeed, _rainAnimationSpeed);
        }

        private void SetPuddlesSettings(Material material)
        {
            if (material.HasProperty(PuddlesNormal))
                material.SetTexture(PuddlesNormal, _puddleNormal);

            if (material.HasProperty(PuddlesFramesSize))
                material.SetVector(PuddlesFramesSize, _puddlesFramesSize);
            
            if (material.HasProperty(PuddlesSize))
                material.SetFloat(PuddlesSize, _puddlesSize);
            
            if (material.HasProperty(PuddlesAnimationSpeed))
                material.SetFloat(PuddlesAnimationSpeed, _puddlesAnimationSpeed);
        }

        public void AddChildMaterial(Material material)
        {
            if(!_childMaterials.Contains(material))
                _childMaterials.Add(material);
        }
        
        public void RemoveChildMaterial(Material material)
        {
            if(_childMaterials.Contains(material))
                _childMaterials.Remove(material);
        }
    }
}