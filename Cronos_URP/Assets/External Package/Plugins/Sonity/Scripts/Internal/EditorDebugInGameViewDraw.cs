// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public static class EditorDebugInGameViewDraw {

        private static GUIStyle guiStyle = new GUIStyle();
        private static GUIContent guiContent = new GUIContent();
        private static Vector2 guiContentSize;
        private static Vector3 screenPos;
        private static Vector2 textSize;
        private static bool hasWarnedNullCamera = false;

        // Older version doesn't have the SubsystemRegistration load type
#if UNITY_2019_2_OR_NEWER
        // Needed for disabling domain reloading
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RuntimeInitializeOnLoad() {
            hasWarnedNullCamera = false;
        }
#endif

        public static void Draw(
            string text, Vector3 worldPos, Color lifetimeStartColor, Color lifetimeFadeColor, Color outlineColor, 
            float timePlayed, float volumeToAlpha, float lifetimeToAlpha, float lifetimeFadeLength, 
            float voiceVolume, int fontSize, bool outline = true
            ) {

            if (Camera.main == null) {
                if (!hasWarnedNullCamera && ShouldDebug.Warnings()) {
                    hasWarnedNullCamera = true;
                    Debug.LogWarning("Sonity.SoundManager \"Debug SoundEvents Live in Game View\": Cant render because Camera.main is null");
                }
                return;
            }

            if (lifetimeFadeLength == 0f) {
                lifetimeFadeLength = 0.00001f;
            }
            float timeAlpha = 1f - Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f);
            float textAlpha = (timeAlpha -1f) * lifetimeToAlpha + 1f;
            textAlpha *= (LogLinExp.Get(voiceVolume, -2f) - 1f) * volumeToAlpha + 1f;

            guiStyle.fontSize = Mathf.RoundToInt(fontSize * 1.8f);
            guiStyle.alignment = TextAnchor.MiddleCenter;

            guiContent.text = text;
            guiContentSize = guiStyle.CalcSize(guiContent);

            screenPos = Camera.main.WorldToScreenPoint(worldPos);
            textSize = GUI.skin.label.CalcSize(guiContent);

            // If outside of screen
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x + guiContentSize.x < 0 || screenPos.x - guiContentSize.x > Screen.width || screenPos.z < 0) {
                return;
            }

            if (outline) {
                // Alpha divided by 4
                outlineColor.a *= textAlpha * 0.25f;
                guiStyle.normal.textColor = outlineColor;
                float shadowOffset = 1f;
                // Below
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f + shadowOffset, textSize.x, textSize.y), guiContent, guiStyle);
                // Above
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f - shadowOffset, textSize.x, textSize.y), guiContent, guiStyle);
                // Left
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f + shadowOffset, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
                // Right
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f - shadowOffset, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
            }

            // Colored text
            lifetimeStartColor = Color.Lerp(lifetimeStartColor, lifetimeFadeColor, Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f));
            lifetimeStartColor.a *= textAlpha;
            guiStyle.normal.textColor = lifetimeStartColor;
            GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
        }
    }
}
#endif