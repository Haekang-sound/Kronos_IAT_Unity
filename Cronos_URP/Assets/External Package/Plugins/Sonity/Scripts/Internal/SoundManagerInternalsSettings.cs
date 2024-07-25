// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {
    
    [Serializable]
    public class SoundManagerInternalsSettings {

        public bool settingExpand = true;

        public bool disablePlayingSounds = false;

        public SoundTimeScaleMode soundTimeScale = SoundTimeScaleMode.UnscaledTime;

        public bool globalPause = false;

        public float globalVolumeRatio = 1f;
        // Decibel only used in the editor
        public float globalVolumeDecibel = 0f;

        public bool dontDestroyOnLoad = true;
        public bool debugWarnings = true;
        public bool debugInPlayMode = true;

        public bool GetShouldDebugWarnings() {
            if (debugWarnings) {
                if (Application.isPlaying) {
                    return debugInPlayMode;
                } else {
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        public bool guiWarnings = true;
        public bool GetShouldDebugGuiWarnings() {
            return guiWarnings;
        }
#endif

        public SoundTagBase globalSoundTag;
        public float distanceScale = 1f;
        public bool overrideListenerDistance = false;
        public float overrideListenerDistanceAmount = 100f;
        public bool speedOfSoundEnabled = false;
        public float speedOfSoundScale = 1f;

        /// <summary>
        /// Returns the Sound of Speed delay in seconds
        /// Distance is the unscaled distance between the <see cref="AudioListener"/> and the <see cref="Voice"/>
        /// </summary>
        public float GetSpeedOfSoundDelay(float distance) {
            if (speedOfSoundEnabled) {
                // Base value is 340 unity units per second. 1/340 = 0.00294117647058823529
                return distance * 0.002941f * speedOfSoundScale;
            } else {
                return 0f;
            }
        }

        public int voicePreload = 32;
        public int voiceLimit = 32;
        public float voiceStopTime = 5f;
        public int voiceEffectLimit = 32;

        // Debug SoundEvents
#if UNITY_EDITOR
        public bool debugSoundEventsInSceneViewEnabled = false;
        public bool debugSoundEventsInGameViewEnabled = false;
        public int debugSoundEventsFontSize = 16;
        public float debugSoundEventsVolumeToAlpha = 0.5f;
        public float debugSoundEventsLifetimeToAlpha = 0.75f;
        public float debugSoundEventsLifetimeColorLength = 1f;
        public Color debugSoundEventsColorStart = EditorColor.GetVolumeMax(1f);
        public Color debugSoundEventsColorEnd = EditorColor.GetVolumeMin(1f);
        public Color debugSoundEventsColorOutline = Color.black;
#endif
    }
}