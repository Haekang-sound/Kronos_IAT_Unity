using System;
using KeroTools.URPPlus.Editor.Drawing;
using UnityEngine;
using UnityEditor;

namespace KeroTools.URPPlus.Editor
{
    internal class KeroEditor
    {
        private readonly KeroEditorUtils _editorUtils;
        private readonly FloatDrawing _floatDrawing;
        private readonly VectorDrawing _vectorDrawing;
        private readonly TextureDrawing _textureDrawing;
        private readonly PopupDrawing _popupDrawing;
        
        public KeroEditor()
        {
            _editorUtils = new KeroEditorUtils();
            
            _floatDrawing = new FloatDrawing(_editorUtils);
            _vectorDrawing = new VectorDrawing(_editorUtils);
            _textureDrawing = new TextureDrawing(_editorUtils);
            _popupDrawing = new PopupDrawing();
        }
        
        public void DrawVertical(GUIStyle styles, Action drawCall) =>
            _editorUtils.DrawVertical(styles, drawCall);
        
        public void DrawIndentedGroup(int level, bool isDisabled, Action drawCall) =>
            _editorUtils.DrawIndentedGroup(level, isDisabled, drawCall);
        
        public void DrawIndented(int level, Action drawCall) =>
            _editorUtils.DrawIndented(level, drawCall);
        
        public void DrawGroup(GUIContent label, Action drawCall) =>
            _editorUtils.DrawGroup(label, drawCall);

        public void DrawGroup(GUIContent label, bool isDisabled, Action drawCall) =>
            _editorUtils.DrawGroup(label, isDisabled, drawCall);
        
        public void DrawDisabledGroup(bool isDisabled, Action drawCall) =>
            _editorUtils.DrawDisabledGroup(isDisabled, drawCall);
        
        public void DrawFloat(GUIContent label, SerializedProperty property, int indentLevel = 0) =>
            _floatDrawing.DrawFloat(label, property, new Vector2(float.MinValue, float.MaxValue), indentLevel);

        public void DrawFloat(GUIContent label, SerializedProperty property, float min, int indentLevel = 0) =>
            _floatDrawing.DrawFloat(label, property, new Vector2(min, float.MaxValue), indentLevel);

        public void DrawSlider(GUIContent label, SerializedProperty property, int indentLevel = 0) =>
            _floatDrawing.DrawSlider(label, property, new Vector2(0.0f, 1.0f), indentLevel);
        
        public void DrawSlider(GUIContent label, SerializedProperty property, Vector2 minMax, int indentLevel = 0) =>
            _floatDrawing.DrawSlider(label, property, minMax, indentLevel);

        public void DrawToggle(GUIContent label, SerializedProperty property, string keyword, int indentLevel = 0) =>
            _floatDrawing.DrawFloatToggle(label, property, keyword, indentLevel);

        public void DrawBooleanPopup(GUIContent label, SerializedProperty property, string[] displayedOptions, string keyword) =>
            _popupDrawing.DrawBooleanPopup(label, property, displayedOptions, keyword);

        public void DrawVector2(GUIContent label, SerializedProperty property, int indentLevel = 0) =>
            _vectorDrawing.DrawVector2(label, property, indentLevel);

        public void DrawColor(GUIContent label, SerializedProperty property) =>
            _vectorDrawing.DrawColor(label, property);

        public void DrawTexture(GUIContent label, SerializedProperty property) =>
            _textureDrawing.DrawTexture(label, property);

        public void DrawSmallTextureField(GUIContent label, SerializedProperty property) =>
            _textureDrawing.DrawSmallTextureField(label, property);
    }
}