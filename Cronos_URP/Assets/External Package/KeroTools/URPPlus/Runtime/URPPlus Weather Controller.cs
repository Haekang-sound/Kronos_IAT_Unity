using System;
using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Runtime
{
    public class URPPlusWeatherController : MonoBehaviour
    {
        [SerializeField] private Color _wetnessColor;
        [SerializeField] [Range(0.0f, 1f)] private float _rainMultiplier = 0.0f;
        [SerializeField] [Range(0.0f, 1f)] private float _wetness = 0.0f;
       
        public static bool IsDisabled { get; private set; }
#if UNITY_EDITOR
        private URPPlusWeatherController() => 
            EditorApplication.delayCall += UpdateKeywords;
#endif
        private void OnEnable() => 
            IsDisabled = false;

        private void OnDisable()
        {
            ResetKeywords();
            IsDisabled = true;
        }
#if UNITY_EDITOR
        private void OnDestroy() => 
            EditorApplication.delayCall -= UpdateKeywords;
#endif
        private void OnValidate() => 
            UpdateKeywords();

        private void UpdateKeywords()
        {
            Shader.SetGlobalFloat(GlobalVariables.RainMultiplier, _rainMultiplier);
            Shader.SetGlobalFloat(GlobalVariables.Wetness, _wetness);
            Shader.SetGlobalColor(GlobalVariables.WetnessColor, _wetnessColor * (1.0f / Mathf.PI));
        }
        
        private void ResetKeywords()
        {
            Shader.SetGlobalFloat(GlobalVariables.RainMultiplier, 0.0f);
            Shader.SetGlobalFloat(GlobalVariables.Wetness, 0.0f);
            Shader.SetGlobalColor(GlobalVariables.WetnessColor, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }
        
        public static void SetKeyword(string keyword, bool enable)
        {
            if (enable)
                Shader.EnableKeyword(keyword);
            else
                Shader.DisableKeyword(keyword);
        }
    }
}