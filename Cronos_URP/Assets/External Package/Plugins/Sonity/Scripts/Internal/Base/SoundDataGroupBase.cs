// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundDataGroupBase : ScriptableObject {

        public SoundDataGroupInternals internals = new SoundDataGroupInternals();
    }
}