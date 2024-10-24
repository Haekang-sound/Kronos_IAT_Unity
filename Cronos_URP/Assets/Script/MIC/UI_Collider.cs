using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider : MonoBehaviour
{
    // 가장 간단한 콜라이더로 목표UI를 띄우는 방법
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            UIManager.Instance.StartCoroutine(UIManager.Instance.ShowRegionNameCoroutine());
            Destroy(gameObject);
        }

    }
}
