// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorDebugInGameView {

        public static void Update() {
            if (SoundManagerBase.Instance != null && SoundManagerBase.Instance.Internals.settings.debugSoundEventsInGameViewEnabled) {
                if (SoundManagerBase.Instance.Internals.voicePool.voicePool == null) {
                    return;
                }
                for (int i = 0; i < SoundManagerBase.Instance.Internals.voicePool.voicePool.Length; i++) {
                    if (SoundManagerBase.Instance.Internals.voicePool.voicePool[i] != null
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].isAssigned
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].soundEvent != null
                        && SoundManagerBase.Instance.Internals.voicePool.voicePool[i].cachedGameObject != null) {
                        if (SoundManagerBase.Instance.Internals.voicePool.voicePool[i].GetState() == VoiceState.Play) {
                            EditorDebugInGameViewDraw.Draw(
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