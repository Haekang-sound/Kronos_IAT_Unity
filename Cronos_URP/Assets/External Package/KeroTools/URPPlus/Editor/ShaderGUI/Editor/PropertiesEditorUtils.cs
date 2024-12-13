using System;

using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor.ShaderGUI.Editor
{
    public class PropertiesEditorUtils
    {
        public void ContainProperty(MaterialProperty property, Action drawCall)
        {
            if (property is null)
                return;
            
            MaterialEditor.BeginProperty(property);
            drawCall.Invoke();
            MaterialEditor.EndProperty();
        }
        
        public void DrawIndented(Action drawCall) => 
            DrawIndented(drawCall, 1);
        
        public void DrawIndented(Action drawCall, uint level = 0)
        {
            EditorGUI.indentLevel += (int)level;
            drawCall.Invoke();
            EditorGUI.indentLevel -= (int)level;
        }

        public bool IsMobilePlatform() => 
            UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget);
        
        public Rect GetRect(MaterialProperty prop)
        {
            return EditorGUILayout.GetControlRect(true, MaterialEditor.GetDefaultPropertyHeight(prop), EditorStyles.layerMaskField);
        }
    }
}
