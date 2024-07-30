using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeroTools.URPPlus.Runtime.Profiles
{
    [CreateAssetMenu(fileName = "Diffusion Profile", menuName = "Rendering/URP+/Diffusion Profile")]
    public class DiffusionProfileSettings : ScriptableObject, IProfile
    {
        [FormerlySerializedAs("diffusionLUT")] 
        public Texture2D _diffusionLUT;
        
        [FormerlySerializedAs("translucencyColor")] [ColorUsage(false)]
        public Color _translucencyColor;
        [FormerlySerializedAs("translucencyScale")] [Min(0.0f)]
        public float _translucencyScale;
        [FormerlySerializedAs("translucencyPower")] [Range(0.0f, 1.0f)]
        public float _translucencyPower;
        [FormerlySerializedAs("translucencyAmbient")] [Min(0.0f)]
        public float _translucencyAmbient;
        [FormerlySerializedAs("translucencyDistortion")] [Range(0.0f, 1.0f)]
        public float _translucencyDistortion;
        [FormerlySerializedAs("translucencyShadows")] [Range(0.0f, 1.0f)]
        public float _translucencyShadows;
        [FormerlySerializedAs("translucencyDiffuseInfluence")] [Range(0.0f, 1.0f)]
        public float _translucencyDiffuseInfluence;

        [SerializeField] private List<Material> _childMaterials;
        
        [FormerlySerializedAs("hash")] public uint _hash;
        
        private static readonly int TranslucencyColor = Shader.PropertyToID("_DiffusionColor");
        private static readonly int DiffusionLUT = Shader.PropertyToID("_DiffusionLUT");
        private static readonly int TranslucencyScale = Shader.PropertyToID("_TranslucencyScale");
        private static readonly int TranslucencyPower = Shader.PropertyToID("_TranslucencyPower");
        private static readonly int TranslucencyAmbient = Shader.PropertyToID("_TranslucencyAmbient");
        private static readonly int TranslucencyDistortion = Shader.PropertyToID("_TranslucencyDistortion");
        private static readonly int TranslucencyShadows = Shader.PropertyToID("_TranslucencyShadows");
        private static readonly int TranslucencyDiffuseInfluence = Shader.PropertyToID("_TranslucencyDiffuseInfluence");
        
        public DiffusionProfileSettings() => 
            ResetToDefault();

        private void OnValidate() => 
            SetMaterialsProfile();

        public void ResetToDefault()
        {
            _translucencyColor = Color.white;
            _diffusionLUT = null;
            
            _translucencyScale = 1.0f;
            _translucencyPower = 0.05f;
            _translucencyAmbient = 0.0f;
            _translucencyDistortion = 0.0f;
            _translucencyShadows = 0.5f;
            _translucencyDiffuseInfluence = 0.5f;

            SetMaterialsProfile();
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
            if (material.HasProperty(TranslucencyColor))
                material.SetColor(TranslucencyColor, _translucencyColor);
            
            if (material.HasProperty(DiffusionLUT))
                material.SetTexture(DiffusionLUT, _diffusionLUT);

            if (material.HasProperty(TranslucencyScale))
                material.SetFloat(TranslucencyScale, _translucencyScale);
            
            if (material.HasProperty(TranslucencyPower))
                material.SetFloat(TranslucencyPower, _translucencyPower);
            
            if (material.HasProperty(TranslucencyAmbient))
                material.SetFloat(TranslucencyAmbient, _translucencyAmbient);
            
            if (material.HasProperty(TranslucencyDistortion))
                material.SetFloat(TranslucencyDistortion, _translucencyDistortion);
            
            if (material.HasProperty(TranslucencyShadows))
                material.SetFloat(TranslucencyShadows, _translucencyShadows);
            
            if (material.HasProperty(TranslucencyDiffuseInfluence))
                material.SetFloat(TranslucencyDiffuseInfluence, _translucencyDiffuseInfluence);
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