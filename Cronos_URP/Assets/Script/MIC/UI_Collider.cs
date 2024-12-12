using UnityEngine;


/// <summary>
/// 단순하게, 콜라이더에 부딪혔을 때 목표 UI를 띄우는 클래스
/// 박스 콜라이더만 조절하면 되니까 편리하다
/// </summary>
public class UI_Collider : MonoBehaviour
{
    public int questNum;

    private QuestManager qm;

    private void Start()
    {
        qm = QuestManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            qm.StartCoroutine(qm.CallingQuest(questNum));
            Destroy(gameObject);
        }
    }
}
