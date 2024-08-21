using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeSettings : ScriptableObject
{
    public VisualTreeAsset behaviourTreeXml;
    public StyleSheet behaviourTreeStyle;
    public VisualTreeAsset nodeXml;

    static BehaviorTreeSettings FindSettings()
    {
        var guids = AssetDatabase.FindAssets("t:BehaviorTreeSettings");
        if (guids.Length > 1)
        {
            Debug.LogWarning($"Found multiple settings files, using the first.");
        }

        switch (guids.Length)
        {
            case 0:
                return null;
            default:
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<BehaviorTreeSettings>(path);
        }
    }

    internal static BehaviorTreeSettings GetOrCreateSettings()
    {
        var settings = FindSettings();
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<BehaviorTreeSettings>();
            AssetDatabase.CreateAsset(settings, "Assets");
            AssetDatabase.SaveAssets();
        }
        return settings;
    }
    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
}
