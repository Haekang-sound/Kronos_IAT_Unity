using System.Collections;
using UnityEngine;

/// <summary>
/// 트리거 되면 오브젝트 Achieve을 호출하는 단순한 클래스
/// </summary>
public class ObjectAchiever : MonoBehaviour
{
    private UIManager um;
    private QuestManager qm;
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
