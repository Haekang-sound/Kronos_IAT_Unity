using UnityEditor;
using KeroTools.URPPlus.Runtime.Profiles;

namespace KeroTools.URPPlus.Editor.Profiles
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DiffusionProfileSettings))]
    public class DiffuseProfileEditor : UnityEditor.Editor
    {
        private KeroEditor _keroEditor;
        
        private SerializedProperty _diffusionLUT;
        private SerializedProperty _translucencyColor;
        private SerializedProperty _translucencyScale;
        private SerializedProperty _translucencyPower;
        private SerializedProperty _translucencyAmbient;
        private SerializedProperty _translucencyDistortion;
        private SerializedProperty _translucencyShadows;
        private SerializedProperty _translucencyDiffuseInfluence;

        public void OnEnable()
        {
            _keroEditor = new KeroEditor();
            
            FindDiffusionProperties();
            FindTranlucencyProperties();
        }

        private void FindDiffusionProperties() => 
            _diffusionLUT = serializedObject.FindProperty("_diffusionLUT");

        private void FindTranlucencyProperties()
        {
            _translucencyColor = serializedObject.FindProperty("_translucencyColor");
            _translucencyScale = serializedObject.FindProperty("_translucencyScale");
            _translucencyPower = serializedObject.FindProperty("_translucencyPower");
            _translucencyAmbient = serializedObject.FindProperty("_translucencyAmbient");
            _translucencyDistortion = serializedObject.FindProperty("_translucencyDistortion");
            _translucencyShadows = serializedObject.FindProperty("_translucencyShadows");
            _translucencyDiffuseInfluence = serializedObject.FindProperty("_translucencyDiffuseInfluence");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDiffusionProperties();
            DrawTranlucencySettings();
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawDiffusionProperties()
        {
            _keroEditor.DrawGroup(DiffuseProfileStyles.DiffusionSettingsLabel,  () =>
            {
                _keroEditor.DrawTexture(DiffuseProfileStyles.DiffusionLUT, _diffusionLUT);
            });
        }

        private void DrawTranlucencySettings()
        {
            _keroEditor.DrawGroup(DiffuseProfileStyles.TranlucencySettingsLabel,  () =>
            {
                _keroEditor.DrawColor(DiffuseProfileStyles.TranlucencyColor, _translucencyColor);
                _keroEditor.DrawFloat(DiffuseProfileStyles.TranlucencyIntensity, _translucencyScale, 0.0f);
                _keroEditor.DrawSlider(DiffuseProfileStyles.TranlucencyPower, _translucencyPower);
                _keroEditor.DrawFloat(DiffuseProfileStyles.TranlucencyAmbient, _translucencyAmbient, 0.0f);
                _keroEditor.DrawSlider(DiffuseProfileStyles.TranlucencyDistortion, _translucencyDistortion);
                _keroEditor.DrawSlider(DiffuseProfileStyles.TranlucencyShadows, _translucencyShadows);
                _keroEditor.DrawSlider(DiffuseProfileStyles.TranlucencyDiffuseInfluence, _translucencyDiffuseInfluence);
            });
        }
    }
}
