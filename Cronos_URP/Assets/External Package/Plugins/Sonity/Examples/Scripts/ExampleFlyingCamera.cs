// Created by Victor Engstr?
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if ENABLE_INPUT_SYSTEM
// The new Input System

using UnityEngine;
using UnityEngine.InputSystem;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleFlyingCamera : MonoBehaviour {

        // Use WASD to move forward, left, back, right
        // Use Q/E to move up/down
        // Use Shift to move faster
        // Use the Right Mouse Button to look around

        private float mainSpeed = 5000f;
        private float shiftMultiply = 10;
        private float shiftMultiplyCurrent = 1f;
        private float mouseSpeed = 0.1f;
        private float yaw = 0.0f;
        private float pitch = 0.0f;
        private Rigidbody cachedRigidbody;

        void Start() {
            // Sets gravity, in case it was changed in the preferences
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
            cachedRigidbody = gameObject.AddComponent<Rigidbody>();
            cachedRigidbody.useGravity = false;
            cachedRigidbody.drag = 4f;
        }

        void Update() {
            // For looking around
            if (Mouse.current.rightButton.isPressed) {
                yaw += Mouse.current.delta.ReadValue().x * mouseSpeed;
                pitch -= Mouse.current.delta.ReadValue().y * mouseSpeed;
                // private float mouseSpeed = 500f;
                //yaw += Mouse.current.delta.ReadValue().x * Time.deltaTime * mouseSpeed * 0.1f;
                //pitch -= Mouse.current.delta.ReadValue().y * Time.deltaTime * mouseSpeed * 0.1f;
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
        }

        void FixedUpdate() {
            // For moving the camera
            if (Keyboard.current[Key.LeftShift].isPressed) {
                shiftMultiplyCurrent = shiftMultiply;
            } else {
                shiftMultiplyCurrent = 1f;
            }
            if (Keyboard.current[Key.W].isPressed) {
                cachedRigidbody.AddRelativeForce(Vector3.forward * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Keyboard.current[Key.A].isPressed) {
                cachedRigidbody.AddRelativeForce(Vector3.left * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Keyboard.current[Key.S].isPressed) {
                cachedRigidbody.AddRelativeForce(Vector3.back * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Keyboard.current[Key.D].isPressed) {
                cachedRigidbody.AddRelativeForce(Vector3.right * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Keyboard.current[Key.Q].isPressed) {
                cachedRigidbody.AddForce(Vector3.up * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Keyboard.current[Key.E].isPressed) {
                cachedRigidbody.AddForce(Vector3.down * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleFlyingCamera : MonoBehaviour {

        // Use WASD to move forward, left, back, right
        // Use Q/E to move up/down
        // Use Shift to move faster
        // Use the Right Mouse Button to look around

        private float mainSpeed = 5000f;
        private float shiftMultiply = 10;
        private float shiftMultiplyCurrent = 1f;
        private float mouseSpeed = 500f;
        private float yaw = 0.0f;
        private float pitch = 0.0f;
        private Rigidbody cachedRigidbody;

        void Start() {
            // Sets gravity, in case it was changed in the preferences
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
            cachedRigidbody = gameObject.AddComponent<Rigidbody>();
            cachedRigidbody.useGravity = false;
            cachedRigidbody.drag = 4f;
        }

        void Update() {
            // For looking around
            if (Input.GetKey(KeyCode.Mouse1)) {
                yaw += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
                pitch -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
        }

        void FixedUpdate() {
            // For moving the camera
            if (Input.GetKey(KeyCode.LeftShift)) {
                shiftMultiplyCurrent = shiftMultiply;
            } else {
                shiftMultiplyCurrent = 1f;
            }
            if (Input.GetKey(KeyCode.W)) {
                cachedRigidbody.AddRelativeForce(Vector3.forward * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.A)) {
                cachedRigidbody.AddRelativeForce(Vector3.left * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.S)) {
                cachedRigidbody.AddRelativeForce(Vector3.back * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.D)) {
                cachedRigidbody.AddRelativeForce(Vector3.right * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.Q)) {
                cachedRigidbody.AddForce(Vector3.up * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.E)) {
                cachedRigidbody.AddForce(Vector3.down * mainSpeed * shiftMultiplyCurrent * Time.fixedDeltaTime);
            }
        }
    }
}
#endif