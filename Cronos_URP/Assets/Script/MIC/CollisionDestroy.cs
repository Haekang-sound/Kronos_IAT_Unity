using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroy : MonoBehaviour
{
    public string wallLayer;

    private void Start()
    {
        wallLayer = "Wall";
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered" + other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer(wallLayer))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
