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
    public class ExampleParameter : MonoBehaviour {

        public SoundEvent soundEvent;
        public SoundParameterPitchSemitone parameterPitch = new SoundParameterPitchSemitone(0f, UpdateMode.Continuous);

        void Update() {

            if (Mouse.current.leftButton.wasPressedThisFrame) {

                // SoundParameter lower pitch
                parameterPitch.PitchSemitone -= 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);

            } else if (Mouse.current.rightButton.wasPressedThisFrame) {

                // SoundParameter increase pitch
                parameterPitch.PitchSemitone += 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press mouse left/right to increase/lower pitch\nCurrent pitch is " + parameterPitch.PitchSemitone.ToString("0") + " semitones";
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleParameter : MonoBehaviour {

        public SoundEvent soundEvent;
        public SoundParameterPitchSemitone parameterPitch = new SoundParameterPitchSemitone(0f, UpdateMode.Continuous);

        void Update() {

            if (Input.GetKeyDown(KeyCode.Mouse0)) {

                // SoundParameter lower pitch
                parameterPitch.PitchSemitone -= 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);

            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {

                // SoundParameter increase pitch
                parameterPitch.PitchSemitone += 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press mouse left/right to increase/lower pitch\nCurrent pitch is " + parameterPitch.PitchSemitone.ToString("0") + " semitones";
        }
    }
}
#endif