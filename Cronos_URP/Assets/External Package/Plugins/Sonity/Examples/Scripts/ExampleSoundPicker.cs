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
    public class ExampleSoundPicker : MonoBehaviour {

        public bool exampleBool;

        public SoundPicker soundPicker;

        public float exampleFloat;

        void Update() {
            // Plays the sound on left mouse click
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                soundPicker.Play(transform);
            }
            // Stops the sound on right mouse click
            if (Mouse.current.rightButton.wasPressedThisFrame) {
                soundPicker.Stop(transform);
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
    public class ExampleSoundPicker : MonoBehaviour {

        public bool exampleBool;

        public SoundPicker soundPicker;
        
        public float exampleFloat;

        void Update() {
            // Plays the sound on left mouse click
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                soundPicker.Play(transform);
            }
            // Stops the sound on right mouse click
            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                soundPicker.Stop(transform);
            }
        }
    }
}

#endif