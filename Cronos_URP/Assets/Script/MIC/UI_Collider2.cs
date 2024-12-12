using UnityEngine;


/// <summary>
/// 2번째 스테이지 리전을 팝업하는 매우 러프한 클래스
/// 트리거 박스 안에 플레이어가 있다면 작동한다
/// </summary>
public class UI_Collider2 : MonoBehaviour
{
    public int regionNum;

    private UIManager um;
    private QuestManager qm;

    // Start is called before the first frame update
    void Start()
    {
        um = UIManager.Instance;

        qm = QuestManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            um.StartRegion(regionNum);
            Debug.Log("STAY");
            Destroy(gameObject);
        }
    }
}
