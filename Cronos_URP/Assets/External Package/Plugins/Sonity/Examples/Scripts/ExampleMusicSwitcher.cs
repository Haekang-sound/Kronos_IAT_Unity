// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if ENABLE_INPUT_SYSTEM
// The new Input System

using UnityEngine;
using UnityEngine.InputSystem;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleMusicSwitcher : MonoBehaviour {

        private void Update() {

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                // Play main menu music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayMainMenu();
            } else if (Mouse.current.rightButton.wasPressedThisFrame) {
                // Play ingame music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayIngame();
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto play main menu/ingame music\nCurrent music is " + SonityTemplate.TemplateSoundMusicManager.Instance.GetMusicPlaying();
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleMusicSwitcher : MonoBehaviour {

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                // Play main menu music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayMainMenu();
            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
                // Play ingame music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayIngame();
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto play main menu/ingame music\nCurrent music is " + SonityTemplate.TemplateSoundMusicManager.Instance.GetMusicPlaying();
        }
    }
}
#endif

