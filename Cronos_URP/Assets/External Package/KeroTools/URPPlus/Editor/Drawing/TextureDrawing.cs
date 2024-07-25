using UnityEditor;
using UnityEngine;

namespace KeroTools.URPPlus.Editor.Drawing
{
    public class TextureDrawing
    {
        private readonly KeroEditorUtils _editorUtils;

        public TextureDrawing(KeroEditorUtils editorUtils) =>
            _editorUtils = editorUtils;
        
        public void DrawTexture(GUIContent label, SerializedProperty property)
        {
            if (property is null)
                return;

            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            var newValue = EditorGUILayout.ObjectField(label, (Texture2D)property.objectReferenceValue, 
                typeof(Texture2D), false) as Texture2D;
            EditorGUI.showMixedValue = false;

            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = newValue;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        
        public void DrawSmallTextureField(GUIContent label, SerializedProperty property)
        {
            if (property is null) 
                return;
            
            const float thumbnailSize = 16f;

            var thumbnailRect = EditorGUILayout.GetControlRect(false, thumbnailSize);
            var texture = property.objectReferenceValue as Texture2D;

            EditorGUI.DrawTextureTransparent(thumbnailRect, texture, ScaleMode.ScaleToFit);
            EditorGUI.PropertyField(thumbnailRect, property, label);
        }
    }
}