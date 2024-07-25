// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR
// Code for older because SceneView.duringSceneGui doesnt exist
#if UNITY_2019_1_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;

namespace Sonity.Internal {

    public static class EditorDebugInSceneView {

        [NonSerialized]
        private static bool debugInSceneViewAdded = false;

        // Older version doesn't have the SubsystemRegistration load type
#if UNITY_2019_2_OR_NEWER
        // Needed for disabling domain reloading
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RuntimeInitializeOnLoad() {
            debugInSceneViewAdded = false;
            if (!debugInSceneViewAdded) {
                debugInSceneViewAdded = true;
                SceneView.duringSceneGui += Update;
            }
        }
#endif

        private static void Update(SceneView sceneview) {
            if (SoundManagerBase.Instance != null && SoundManagerBase.Instance.Internals.settings.debugSoundEventsInSceneViewEnabled) {
                if (SoundManagerBase.Instance.Internals.voicePool.voicePool == null) {
                    return;
                }
                for (int i = 0; i < SoundManagerBase.Instance.Internals.voicePool.voicePool.Length; i++) {
                    if (SoundManagerBase.Instance.Internals.voicePool.voicePool[i] != null
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].isAssigned
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].soundEvent != null
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].cachedGameObject != null) {
                        if (SoundManagerBase.Instance.Internals.voicePool.voicePool[i].GetState() == VoiceState.Play) {
                            EditorDebugInSceneViewDraw.Draw(
                                SoundManagerBase.Instance.Internals.voicePool.voicePool[i].soundEventName,
                                SoundManagerBase.Instance.Internals.voicePool.voicePool[i].cachedGameObject.transform.position,
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsColorStart,
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsColorEnd,
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsColorOutline,
                                SoundManagerBase.Instance.Internals.voicePool.voicePool[i].GetTimePlayed(),
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsVolumeToAlpha,
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsLifetimeToAlpha,
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsLifetimeColorLength,
                                SoundManagerBase.Instance.Internals.voicePool.voicePool[i].GetVolumeRatioWithFade(),
                                SoundManagerBase.Instance.Internals.settings.debugSoundEventsFontSize
                                );
                        }
                    }
                }
            }
        }
    }
}
#endif
#endif