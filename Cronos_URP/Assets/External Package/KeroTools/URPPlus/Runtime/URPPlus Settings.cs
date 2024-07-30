using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Runtime
{
    [ExecuteInEditMode]
    public class URPPlusSettings : MonoBehaviour
    {
        [Header("Material Quality")]
        [SerializeField] private int _sssModel;

        [SerializeField] private int _iridescenceModel;
        [SerializeField] private int _sheenModel;
        [SerializeField] private bool _enableCoatGeometricAA;
        [SerializeField] [Range(0.001f, 1f)] private float _screenFadeDistance = 0.1f;

        [Header("Ambient Occlusion")] 
        [SerializeField] private bool _enableMicroShadows;

        [SerializeField] [Range(0f, 1f)] private float _microShadowsOpacity;
        [SerializeField] private bool _enableHighQualityDepthNormals;
        
        public static bool useIridescenceLUT;
        public static bool IsDisabled { get; private set; }
        
#if UNITY_EDITOR
        private URPPlusSettings() => 
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

        private void OnValidate() => UpdateKeywords();

        private void UpdateKeywords()
        {
            var sssLUT = _sssModel == 1;
            useIridescenceLUT = _iridescenceModel == 1;
            var sheenMode = _sheenModel == 1;

            SetKeyword(GlobalVariables.PreIntegratedSssKeyword, sssLUT);
            SetKeyword(GlobalVariables.PreIntegratedIridescenceKeyword, useIridescenceLUT);
            SetKeyword(GlobalVariables.PbSheenKeyword, sheenMode);
            SetKeyword(GlobalVariables.CoatSpecularAAKeyword, _enableCoatGeometricAA);
            SetKeyword(GlobalVariables.MicroShadowsKeyword, _enableMicroShadows);
            SetKeyword(GlobalVariables.HqDepthNormalsKeyword, _enableHighQualityDepthNormals);

            Shader.SetGlobalFloat(GlobalVariables.SsRefractionInvScreenWeightDistance, 1.0f / _screenFadeDistance);
            if (_enableMicroShadows)
                Shader.SetGlobalFloat(GlobalVariables.MicroShadowOpacity, _microShadowsOpacity);
        }

        private void ResetKeywords()
        {
            SetKeyword(GlobalVariables.PreIntegratedSssKeyword, false);
            SetKeyword(GlobalVariables.PreIntegratedIridescenceKeyword, false);
            SetKeyword(GlobalVariables.PbSheenKeyword, false);
            SetKeyword(GlobalVariables.CoatSpecularAAKeyword, false);
            SetKeyword(GlobalVariables.MicroShadowsKeyword, false);
            SetKeyword(GlobalVariables.HqDepthNormalsKeyword, false);

            Shader.SetGlobalFloat(GlobalVariables.SsRefractionInvScreenWeightDistance, 10.0f);
            Shader.SetGlobalFloat(GlobalVariables.MicroShadowOpacity, 0.0f);
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