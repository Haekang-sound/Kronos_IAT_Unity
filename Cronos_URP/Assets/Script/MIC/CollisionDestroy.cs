using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어 스킬이 벽에 닿으면 없애는 단순한 클래스
/// </summary>
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
