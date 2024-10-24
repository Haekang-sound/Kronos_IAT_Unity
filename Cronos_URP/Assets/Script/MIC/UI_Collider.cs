using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collider : MonoBehaviour
{
    public int questNum;

    QuestManager qm;

    private void Start()
    {
        qm = QuestManager.Instance;
    }

    // ���� ������ �ݶ��̴��� ��ǥUI�� ���� ���
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            qm.StartCoroutine(qm.CallingQuest(questNum));
            //UIManager.Instance.StartCoroutine(UIManager.Instance.ShowRegionNameCoroutine());
            //Destroy(gameObject);
        }

    }
}
