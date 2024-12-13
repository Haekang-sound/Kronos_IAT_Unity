using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int m_sceneIndex = -1;
    GUIContent[] m_sceneNames;
    readonly string[] k_scenePathSplitters = { "/", ".unity" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;
        if (m_sceneIndex == -1)
            Setup(property);

        int oldIndex = m_sceneIndex;
        m_sceneIndex = EditorGUI.Popup(position, label, m_sceneIndex, m_sceneNames);

        if (oldIndex != m_sceneIndex)
            property.stringValue = m_sceneNames[m_sceneIndex].text;
    }

    void Setup(SerializedProperty property)
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        m_sceneNames = new GUIContent[scenes.Length];

        for (int i = 0; i < m_sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            if (string.IsNullOrEmpty(path))
            {
                m_sceneNames[i] = new GUIContent("INVALID. SCENE WAS DELETED. OPEN BUILD SETTINGS");
            }
            else
            {
                string[] splitPath = path.Split(k_scenePathSplitters, StringSplitOptions.RemoveEmptyEntries);
                m_sceneNames[i] = new GUIContent(splitPath[splitPath.Length - 1]);
            }
        }

        if (m_sceneNames.Length == 0)
            m_sceneNames = new[] { new GUIContent("[No Scenes In Build Settings]") };

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool sceneNameFound = false;
            for (int i = 0; i < m_sceneNames.Length; i++)
            {
                if (m_sceneNames[i].text == property.stringValue)
                {
                    m_sceneIndex = i;
                    sceneNameFound = true;
                    break;
                }
            }
            if (!sceneNameFound)
                m_sceneIndex = 0;
        }
        else m_sceneIndex = 0;

        property.stringValue = m_sceneNames[m_sceneIndex].text;
    }
}
