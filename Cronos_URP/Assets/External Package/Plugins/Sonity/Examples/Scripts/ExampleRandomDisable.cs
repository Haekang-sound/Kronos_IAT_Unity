// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleRandomDisable : MonoBehaviour {

        public GameObject gameObjectToDisable;
        bool isActive = true;
        float timeCurrent = 1f;

        void Update() {
            if (gameObjectToDisable != null) {
                // Toggles the object from enabled to disabled randomly
                if (timeCurrent <= 0f) {
                    isActive = !isActive;
                    gameObjectToDisable.SetActive(isActive);
                    timeCurrent = Random.Range(2f, 3f);
                }
                timeCurrent -= Time.deltaTime;
            }
        }
    }
}