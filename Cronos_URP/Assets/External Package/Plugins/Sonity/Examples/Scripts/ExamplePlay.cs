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
    public class ExamplePlay : MonoBehaviour {

        public SoundEvent soundEvent;

        void Update() {
            // Plays the sound on left mouse click
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                soundEvent.Play(transform);
            }
            // Stops the sound on right mouse click
            if (Mouse.current.rightButton.wasPressedThisFrame) {
                soundEvent.Stop(transform);
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
    public class ExamplePlay : MonoBehaviour {

        public SoundEvent soundEvent;

        void Update() {
            // Plays the sound on left mouse click
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                soundEvent.Play(transform);
            }
            // Stops the sound on right mouse click
            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                soundEvent.Stop(transform);
            }
        }
    }
}  
#endif