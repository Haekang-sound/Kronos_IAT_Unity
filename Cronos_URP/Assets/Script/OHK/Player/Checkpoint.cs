using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public int priority;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Checkpoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();

        if (controller == null)
            return;

        controller.SetCheckpoint(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.75f;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
