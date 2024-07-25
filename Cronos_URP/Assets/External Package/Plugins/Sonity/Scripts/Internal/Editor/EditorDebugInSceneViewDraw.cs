// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR
// Code for older because SceneView.duringSceneGui doesnt exist
#if UNITY_2019_1_OR_NEWER

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public static class EditorDebugInSceneViewDraw {

        private static SceneView view;
        private static Vector3 screenPos;
        private static GUIStyle guiStyle = new GUIStyle();
        private static GUIContent guiContent = new GUIContent();
        private static Vector2 guiContentSize = guiStyle.CalcSize(guiContent);

        static public void Draw(
            string text, Vector3 worldPos, Color lifetimeStartColor, Color lifetimeFadeColor, Color outlineColor,
            float timePlayed, float volumeToAlpha, float lifetimeToAlpha, float lifetimeFadeLength, 
            float voiceVolume, int fontSize, bool outline = true, float offsetX = 0f, float offsetY = 0f
            ) {

            if (lifetimeFadeLength == 0f) {
                lifetimeFadeLength = 0.00001f;
            }
            float timeAlpha = 1f - Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f);
            float textAlpha = (timeAlpha - 1f) * lifetimeToAlpha + 1f;
            textAlpha *= (LogLinExp.Get(voiceVolume, -2f) - 1f) * volumeToAlpha + 1f;

            view = SceneView.currentDrawingSceneView;
            screenPos = view.camera.WorldToScreenPoint(worldPos);

            guiStyle.fontSize = fontSize;
            guiStyle.alignment = TextAnchor.UpperLeft;

            guiContent.text = text;
            guiContentSize = guiStyle.CalcSize(guiContent);

            // If outside of screen
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x + guiContentSize.x < 0 || screenPos.x - guiContentSize.x > Screen.width || screenPos.z < 0) {
                return;
            }

            float textOffsetX = guiContentSize.x * 0.74f;
            float textOffsetY = -guiContentSize.y * 0.9f;

            if (outline) {
                // Alpha divided by 4
                outlineColor.a *= textAlpha * 0.25f;
                guiStyle.normal.textColor = outlineColor;
                float shadowOffset = 1f;
                // Below
                Handles.Label(MoveByPixel(worldPos, offsetX - textOffsetX, offsetY - shadowOffset - textOffsetY), guiContent, guiStyle);
                // Above
                Handles.Label(MoveByPixel(worldPos, offsetX - textOffsetX, offsetY + shadowOffset - textOffsetY), guiContent, guiStyle);
                // Left
                Handles.Label(MoveByPixel(worldPos, offsetX - textOffsetX - shadowOffset, offsetY - textOffsetY), guiContent, guiStyle);
                // Right
                Handles.Label(MoveByPixel(worldPos, offsetX - textOffsetX + shadowOffset, offsetY - textOffsetY), guiContent, guiStyle);
            }

            lifetimeStartColor = Color.Lerp(lifetimeStartColor, lifetimeFadeColor, Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f));
            lifetimeStartColor.a *= textAlpha;
            guiStyle.normal.textColor = lifetimeStartColor;
            Handles.Label(MoveByPixel(worldPos, offsetX - textOffsetX, offsetY - textOffsetY), guiContent, guiStyle);
        }

        private static Vector3 MoveByPixel(Vector3 position, float x, float y) {
            Camera camera = SceneView.currentDrawingSceneView.camera;
            if (camera) {
                return camera.ScreenToWorldPoint(camera.WorldToScreenPoint(position) + new Vector3(x, y));
            } else {
                return position;
            }
        }
    }
}
#endif
#endif