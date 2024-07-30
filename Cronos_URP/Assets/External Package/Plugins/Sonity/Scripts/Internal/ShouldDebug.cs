// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public static class ShouldDebug {

        public static bool Warnings() {
        // For DLL
#if DEBUG || SONITYDLL
            return SoundManagerBase.Instance == null || SoundManagerBase.Instance.Internals.settings.GetShouldDebugWarnings();
#else
            return false;
#endif
        }

#if UNITY_EDITOR
        public static bool GuiWarnings() {
            return SoundManagerBase.Instance == null || SoundManagerBase.Instance.Internals.settings.GetShouldDebugGuiWarnings();
        }
#endif
    }
}