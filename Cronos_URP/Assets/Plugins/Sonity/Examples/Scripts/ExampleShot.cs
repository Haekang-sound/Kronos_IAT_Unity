// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if ENABLE_INPUT_SYSTEM
// The new Input System

using UnityEngine;
using UnityEngine.InputSystem;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleShot : MonoBehaviour {

        public SoundEvent soundEventShot;

        void Update() {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                soundEventShot.Play(transform);
            }
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleShot : MonoBehaviour {

        public SoundEvent soundEventShot;

        void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                soundEventShot.Play(transform);
            }
        }
    }
}
#endif