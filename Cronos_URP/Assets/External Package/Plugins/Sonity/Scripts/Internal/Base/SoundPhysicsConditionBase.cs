// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundPhysicsConditionBase : ScriptableObject {

        public SoundPhysicsConditionInternals internals = new SoundPhysicsConditionInternals();
    }
}
