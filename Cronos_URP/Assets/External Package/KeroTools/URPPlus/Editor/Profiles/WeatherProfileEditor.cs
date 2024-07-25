using KeroTools.URPPlus.Runtime.Profiles;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.Profiles
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WeatherProfileSettings))]
    public class WeatherProfileEditor : UnityEditor.Editor
    {
        private KeroEditor _keroEditor;
        
        private SerializedProperty _puddleNormal;
        private SerializedProperty _puddlesFramesSize;
        private SerializedProperty _puddlesSize;
        private SerializedProperty _puddlesAnimationSpeed;
        
        private SerializedProperty _rainNormal;
        private SerializedProperty _rainSize;
        private SerializedProperty _rainAnimationSpeed;
        
        public void OnEnable()
        {
            _keroEditor = new KeroEditor();
            
            FindPuddlesProperties();
            FindRainProperties();
        }
        
        private void FindPuddlesProperties()
        {
            _puddleNormal = serializedObject.FindProperty("_puddleNormal");
            _puddlesFramesSize = serializedObject.FindProperty("_puddlesFramesSize");
            _puddlesSize = serializedObject.FindProperty("_puddlesSize");
            _puddlesAnimationSpeed = serializedObject.FindProperty("_puddlesAnimationSpeed");
        }

        private void FindRainProperties()
        {
            _rainNormal = serializedObject.FindProperty("_rainNormal");
            _rainSize = serializedObject.FindProperty("_rainSize");
            _rainAnimationSpeed = serializedObject.FindProperty("_rainAnimationSpeed");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPuddlesProperties();
            DrawRainSettings();
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawPuddlesProperties()
        {
            _keroEditor.DrawGroup(WeatherProfileStyles.PuddlesSettingsLabel,  () =>
            {
                _keroEditor.DrawTexture(WeatherProfileStyles.PuddleNormal, _puddleNormal);
                _keroEditor.DrawVector2(WeatherProfileStyles.PuddlesFramesSize, _puddlesFramesSize);
                _keroEditor.DrawFloat(WeatherProfileStyles.PuddlesSize, _puddlesSize, 0.0f);
                _keroEditor.DrawFloat(WeatherProfileStyles.PuddlesAnimationSpeed, _puddlesAnimationSpeed, 0.0f);
            });
        }

        private void DrawRainSettings()
        {
            _keroEditor.DrawGroup(WeatherProfileStyles.RainSettingsLabel,  () =>
            {
                _keroEditor.DrawTexture(WeatherProfileStyles.RainNormal, _rainNormal);
                _keroEditor.DrawFloat(WeatherProfileStyles.RainSize, _rainSize, 0.0f);
                _keroEditor.DrawFloat(WeatherProfileStyles.RainAnimationSpeed, _rainAnimationSpeed, 0.0f);
            });
        }
    }
}