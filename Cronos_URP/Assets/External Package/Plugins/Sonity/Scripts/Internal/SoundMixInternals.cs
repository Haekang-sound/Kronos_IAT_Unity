// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundMixInternals {

        public SoundEventModifier soundEventModifier = new SoundEventModifier();
        public SoundMixBase soundMixParent;

        public bool CheckIsInfiniteLoop(SoundMixBase soundMix, bool isEditor) {
            if (isEditor) {
                bool isInfiniteLoop = GetIfInfiniteLoop(soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious);
                if (isInfiniteLoop) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundMix)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
                return isInfiniteLoop;
            } else {
                return SoundManagerBase.Instance.Internals.InternalCheckSoundMixIsInfiniteLoop(soundMix);
            }
        }

        public bool GetIfInfiniteLoop(SoundMixBase soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious) {

            infiniteInstigator = null;
            infinitePrevious = null;

            if (soundMix == null) {
                return false;
            }

            List<SoundMixBase> toCheck = new List<SoundMixBase>();
            List<SoundMixBase> isChecked = new List<SoundMixBase>();

            toCheck.Add(soundMix);

            while (toCheck.Count > 0) {
                SoundMixBase soundMixChild = toCheck[0];
                toCheck.RemoveAt(0);
                if (soundMixChild != null) {
                    for (int i = 0; i < isChecked.Count; i++) {
                        if (isChecked[i] == soundMixChild) {
                            infiniteInstigator = isChecked[i];
                            return true;
                        }
                    }

                    if (soundMixChild.internals.soundMixParent != null) {
                        toCheck.Add(soundMixChild.internals.soundMixParent);
                        infinitePrevious = soundMixChild;
                    }

                    isChecked.Add(soundMixChild);
                }
            }
            return false;
        }
    }
}