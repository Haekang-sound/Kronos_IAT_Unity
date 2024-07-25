using System;

using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor
{
    public class KeroEditorUtils
    {
        public void DrawVertical(GUIStyle styles, Action drawCall)
        {
            EditorGUILayout.BeginVertical(styles);
            drawCall.Invoke();
            EditorGUILayout.EndVertical();
        }

        public void DrawIndentedGroup(int level, bool isDisabled, Action drawCall) => 
            DrawDisabledGroup(isDisabled, () => DrawIndented(level, drawCall));
        
        public void DrawIndented(int level, Action drawCall)
        {
            EditorGUI.indentLevel += level;
            drawCall.Invoke();
            EditorGUI.indentLevel -= level;
        }
        
        public void DrawGroup(GUIContent styles, Action drawCall) =>
            DrawGroup(styles, false, drawCall);
        
        public void DrawGroup(GUIContent styles, bool isDisabled, Action drawCall)
        {
            DrawVertical(EditorStyles.helpBox, () =>
            {
                EditorGUILayout.LabelField(styles, EditorStyles.boldLabel);
                DrawDisabledGroup(isDisabled, drawCall);
            });
        }
        
        public void DrawDisabledGroup(bool isDisabled, Action drawCall)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            drawCall.Invoke();
            EditorGUI.EndDisabledGroup();
        }
    }
}