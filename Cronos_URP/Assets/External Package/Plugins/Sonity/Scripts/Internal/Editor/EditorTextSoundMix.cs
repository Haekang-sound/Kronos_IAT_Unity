// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTextSoundMix {

        public static string soundMixTooltip =
            $"{nameof(NameOf.SoundMix)} objects are used for grouped control of e.g. volume for multiple {nameof(NameOf.SoundEvent)} at the same time." + "\n" +
            "\n" +
            $"They contain a parent {nameof(NameOf.SoundMix)} and modifiers for the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundMix)} objects are multi-object editable." + "\n" +
            "\n" +
            $"They can be used instead of AudioMixerGroups if you need increased performance." + "\n" +
            "\n" +
            $"Example use; set up a “Master_MIX” and a “SFX_MIX” where the Master_MIX is a parent of the SFX_MIX." + EditorTrial.trialTooltip;
    }
}
#endif