// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundMixBase">SoundMix</see> objects are used for grouped control of e.g. volume for multiple <see cref="SoundEventBase">SoundEvent</see> at the same time.
    /// They contain a parent <see cref="SoundMixBase">SoundMix</see> and modifiers for the <see cref="SoundEventBase">SoundEvent</see>.
    /// All <see cref="SoundMixBase">SoundMix</see> objects are multi-object editable.
    /// They can be used instead of AudioMixerGroups if you need increased performance.
    /// Example use; set up a “Master_MIX” and a “SFX_MIX” where the Master_MIX is a parent of the SFX_MIX.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_MIX", menuName = "Sonity 🔊/SoundMix", order = 103)] // Having a big gap in indexes creates dividers
    public class SoundMix : SoundMixBase {

        /// <summary>
        /// Sets the volume based on decibel
        /// </summary>
        /// <param name="volumeDecibel"> Range NegativeInfinity to 0 </param>
        public void SetVolumeDecibel(float volumeDecibel) {
            Mathf.Clamp(volumeDecibel, Mathf.NegativeInfinity, 0);
            internals.soundEventModifier.volumeDecibel = volumeDecibel;
            internals.soundEventModifier.volumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
        }

        /// <summary>
        /// Sets the pitch based on semitones
        /// </summary>
        /// <param name="pitchSemitone"> No range limit </param>
        public void SetPitchSemitone(float pitchSemitone) {
            internals.soundEventModifier.pitchSemitone = pitchSemitone;
            internals.soundEventModifier.pitchRatio = PitchScale.SemitonesToRatio(pitchSemitone);
        }
    }
}