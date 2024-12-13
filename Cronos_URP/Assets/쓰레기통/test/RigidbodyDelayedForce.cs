using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// rigidebody에 대한 공통적인 설정을 정의.
/// </summary>
public class RigidbodyDelayedForce : MonoBehaviour
{
    public Vector3 forceToAdd;

    private void Start()
    {
        Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rigidbodies.Length; ++i)
        {
            rigidbodies[i].maxAngularVelocity = 45;
            rigidbodies[i].angularVelocity = transform.right * -45.0f;
            rigidbodies[i].velocity = forceToAdd;

        }
    }
}