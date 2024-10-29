using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAchiever : MonoBehaviour
{
    // 단순히 OnTrigger로 오브젝트 Achieve를 호출하는 단순한놈
    UIManager um;
    QuestManager qm;
    public int toCallNum;

    private void Start()
    {
        um = UIManager.Instance;
        qm = QuestManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            StartCoroutine(ShowNextObjective(toCallNum));
        }
    }

    IEnumerator ShowNextObjective(int toCallNum)
    {
        yield return um.AchieveSubObjective();
        Debug.Log("phase 1");
        yield return qm.CallingQuest(toCallNum);
        Debug.Log("phase 2");
        Destroy(gameObject);
    }
}
