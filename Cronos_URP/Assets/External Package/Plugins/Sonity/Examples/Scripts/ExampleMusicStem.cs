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
    public class ExampleMusicStem : MonoBehaviour {

        public SoundEvent musicStem;
        private SoundParameterIntensity parameterIntensity = new SoundParameterIntensity(0f, UpdateMode.Continuous);

        private void Start() {
            musicStem.PlayMusic(true, true, parameterIntensity);
        }

        private void Update() {

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                // Set intensity to low
                parameterIntensity.Intensity = 0f;
            } else if (Mouse.current.rightButton.wasPressedThisFrame) {
                // Set intensity to high
                parameterIntensity.Intensity = 1f;
            }

            // Setting gui text
            if (parameterIntensity.Intensity > 0.5f) {
                GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto change from high to low intensity\nCurrently it is " + "high intensity";
            } else {
                GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto change from high to low intensity\nCurrently it is " + "low intensity";
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
    public class ExampleMusicStem : MonoBehaviour {

        public SoundEvent musicStem;
        private SoundParameterIntensity parameterIntensity = new SoundParameterIntensity(0f, UpdateMode.Continuous);

        private void Start() {
            musicStem.PlayMusic(true, true, parameterIntensity);
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                // Set intensity to low
                parameterIntensity.Intensity = 0f;
            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
                // Set intensity to high
                parameterIntensity.Intensity = 1f;
            }

            // Setting gui text
            if (parameterIntensity.Intensity > 0.5f) {
                GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto change from high to low intensity\nCurrently it is " + "high intensity";
            } else {
                GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto change from high to low intensity\nCurrently it is " + "low intensity";
            }
        }
    }
}
#endif