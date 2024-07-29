// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleColliderSoundTag : MonoBehaviour {

        public SoundTag soundTagIndoor;
        public SoundTag soundTagOutdoor;

        private AudioListener cachedAudioListener;
        private Collider cachedCollider;

        void Start() {
            cachedAudioListener = FindObjectOfType<AudioListener>();
            cachedCollider = GetComponent<Collider>();
        }

        void Update() {
            if (cachedCollider.bounds.Contains(cachedAudioListener.GetComponent<Transform>().position)) {
                // Is Indoor
                SoundManager.Instance.SetGlobalSoundTag(soundTagIndoor);
            } else {
                // Is Outdoor
                SoundManager.Instance.SetGlobalSoundTag(soundTagOutdoor);
            }
        }
    }
}
