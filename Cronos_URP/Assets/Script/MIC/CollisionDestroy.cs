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
        if (other.gameObject.layer == LayerMask.NameToLayer(wallLayer))
        {
            Destroy(transform.parent.gameObject);
            Debug.Log("Invisible Slash triggered Wall");
        }
    }
}
