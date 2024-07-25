using System;

using UnityEngine;
using UnityEditor;

using KeroTools.URPPlus.Runtime;

namespace KeroTools.URPPlus.Editor
{
    [CustomEditor(typeof(URPPlusSettings))]
    public class UrpPlusSettingsEditor : UnityEditor.Editor
    {
        private KeroEditor _keroEditor;
        
        private SerializedProperty _iridescenceModel;
        private SerializedProperty _sheenModel;
        private SerializedProperty _enableCoatGeometricAA;
        private SerializedProperty _screenFadeDistance;
        
        private SerializedProperty _enableMicroShadows;
        private SerializedProperty _microShadowsOpacity;
        private SerializedProperty _enableHighQualityDepthNormals;
        
        private readonly string[] _sheenModelOptions = Enum.GetNames(typeof(SheenModel));
        private readonly string[] _iridescenceModelOptions = Enum.GetNames(typeof(IridescenceModel));
        
        private void OnEnable()
        {
            _keroEditor = new KeroEditor();
            
            _iridescenceModel = serializedObject.FindProperty("_iridescenceModel");
            _sheenModel = serializedObject.FindProperty("_sheenModel");
            _enableCoatGeometricAA = serializedObject.FindProperty("_enableCoatGeometricAA");
            _screenFadeDistance = serializedObject.FindProperty("_screenFadeDistance");

            _enableMicroShadows = serializedObject.FindProperty("_enableMicroShadows");
            _microShadowsOpacity = serializedObject.FindProperty("_microShadowsOpacity");
            _enableHighQualityDepthNormals = serializedObject.FindProperty("_enableHighQualityDepthNormals");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawMaterialsSettings();
            DrawAmbientOcclusionSettings();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMaterialsSettings()
        {
            _keroEditor.DrawGroup(URPPlusStyles.MaterialQualityOptions, URPPlusSettings.IsDisabled, () =>
            {
                DrawIridescenceModel();
                DrawSheenModel();
                DrawCoatSpecularAAToggle();
                _keroEditor.DrawSlider(URPPlusStyles.ScreenFadeDistanceText, _screenFadeDistance, new Vector2(0.001f, 1.0f));
            });
        }

        private void DrawAmbientOcclusionSettings()
        {
            _keroEditor.DrawGroup(URPPlusStyles.MaterialQualityOptions, URPPlusSettings.IsDisabled, () =>
            {
                DrawMicroShadowing();
                DrawHQDepthNormals();
            });
        }

        private void DrawSheenModel()
        {
            _keroEditor.DrawBooleanPopup(URPPlusStyles.SheenModelText, _sheenModel, 
                _sheenModelOptions, GlobalVariables.PbSheenKeyword);
        }

        private void DrawIridescenceModel()
        {
            _keroEditor.DrawBooleanPopup(URPPlusStyles.IridescenceModelText, _iridescenceModel, 
                _iridescenceModelOptions, GlobalVariables.PreIntegratedIridescenceKeyword);
        }

        private void DrawCoatSpecularAAToggle()
        {
            _keroEditor.DrawToggle(URPPlusStyles.CoatSpecularAAText, _enableCoatGeometricAA,
                GlobalVariables.CoatSpecularAAKeyword);
        }

        private void DrawMicroShadowing()
        {
            _keroEditor.DrawToggle(URPPlusStyles.MicroShadowsText, _enableMicroShadows,
                GlobalVariables.MicroShadowsKeyword);
            
            if (_enableMicroShadows.boolValue)
                _keroEditor.DrawSlider(URPPlusStyles.MicroShadowsOpacityText, _microShadowsOpacity, 1);
        }

        private void DrawHQDepthNormals()
        {
            _keroEditor.DrawToggle(URPPlusStyles.HqDepthNormalsText, _enableHighQualityDepthNormals,
                GlobalVariables.HqDepthNormalsKeyword);
        }
    }
}