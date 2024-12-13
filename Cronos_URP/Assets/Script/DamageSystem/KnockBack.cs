using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float ForcePower = 100f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Begin(Vector3 forceSource)
    {
        Vector3 direction = transform.position - forceSource;
        direction.y = 0f;
        _rigidbody.AddForce(direction.normalized * ForcePower, ForceMode.Impulse);
    }
}
