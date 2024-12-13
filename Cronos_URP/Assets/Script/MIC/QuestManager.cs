using System.Collections;
using UnityEngine;


/// <summary>
/// UI로 띄워야 하는 지역과 목표 텍스트를 갖고있는 클래스
/// 새 목표를 띄울 때 현재 목표를 달성했는지 여부를 판단한다
/// </summary>
public class QuestManager : MonoBehaviour
{
    // 호출할 지역 텍스트 배열
    public string[] RegionNames;
    
    // 호출할 퀘스트 텍스트 배열
    public string[] QuestLines;

    // 퀘스트 달성 배열
    [SerializeField]
    private bool curQuesting;
    public bool abilityQuesting = false;

    private UIManager uiManager;

    // 또글턴
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
                if (instance == null)
                {
                    GameObject qManager = new GameObject(typeof(QuestManager).Name);
                    instance = qManager.AddComponent<QuestManager>();

                    DontDestroyOnLoad(qManager);
                }
            }
            return instance;
        }
    }


    // 퀘스트 목표를 호출하는 함수
    // 인덱스번째의 텍스트를 배열에서 불러와 띄울 뿐
    public IEnumerator CallingQuest(int idx)
    {
        // 지금 퀘스트를 끝내지 못한채로 새 목표를 불러냈다면 실패 애니메이션
        if (curQuesting)
        {
            Debug.Log("목표 중 새로운 목표 호출됨");
            yield return uiManager.StartCoroutine(uiManager.FailSubObjective());
        }

        // 현재 퀘스트가 달성이 되었다면 (!curQuesting)이라면
        // 다음 목표로
        uiManager.StartAppearMain(idx);
        // 크로노스 동상을 위해서
        if (idx == 1)
        {
            abilityQuesting = true;
        }
    }

    // 현재 목표 진행중입니다.
    public void Questing()
    {
        curQuesting = true;
    }

    // 현재 진행중인 목표를 달성했든 아니든 끝냈습니다.
    public void QuestDone()
    {
        // ui매니저에서 부르는중
        curQuesting = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.Instance;
    }
}
