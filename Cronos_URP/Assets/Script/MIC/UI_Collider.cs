using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider : MonoBehaviour
{
    // ���� ������ �ݶ��̴��� ��ǥUI�� ���� ���
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            UIManager.Instance.StartCoroutine(UIManager.Instance.ShowRegionNameCoroutine());
            Destroy(gameObject);
        }

    }
}
