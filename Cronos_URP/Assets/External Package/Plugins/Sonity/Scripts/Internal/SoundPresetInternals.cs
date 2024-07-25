// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using System;

#if UNITY_EDITOR

namespace Sonity.Internal {

    [Serializable]
    public class SoundPresetInternals {
        public bool disableAll = false;
        public bool automaticLoop = true;
        public SoundPresetGroup[] soundPresetGroup = new SoundPresetGroup[1];
    }
}
#endif