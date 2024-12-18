using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 지역 이름 및 목표 UI 팝업 및 애니메이션을 담당하는 클래스
/// UI 애니메이션은 보간 그 이상 그 이하도 아니다
/// 퀘스트 매니저와 연동해서 굴러가는 중
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    // Get하는 프로퍼티
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject uiManager = new GameObject(typeof(UIManager).Name);
                    instance = uiManager.AddComponent<UIManager>();

                    DontDestroyOnLoad(uiManager);
                }
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject regionNameObj;
    [SerializeField]
    private Image regionImage;
    [SerializeField]
    private TextMeshProUGUI regionText;
    [SerializeField]
    private GameObject objectiveSubObj;
    [SerializeField]
    private Image subBackImage;
    [SerializeField]
    private TextMeshProUGUI subText;
    [SerializeField]
    private GameObject objectiveMainObj;
    [SerializeField]
    private Image mainBackImage;
    [SerializeField]
    private TextMeshProUGUI mainText;

    private Color texColor = new Color(0.96f, 0.96f, 0.96f);
    private Color acvColor = new Color(0.431f, 0.886f, 0.357f);

    [Tooltip("지역 이름이 페이드되는 시간입니다")]
    public float regionFadeTime = 1.0f;
    [Tooltip("지역 이름이 유지되는 시간입니다")]
    public float regionDurationTime = 2.0f;
    [Tooltip("목표가 페이드되는 시간입니다")]
    public float objFadeTime = 1.0f;
    [Tooltip("목표가 유지되는 시간입니다")]
    public float objDurationTime = 2.0f;

    // 코루틴 중 호출 시 멈추기 위해서
    private Coroutine curCoroutine;

    private QuestManager qm;

    [SerializeField]
    private GameObject interactor;
    // lazy initialization
    // 프로퍼티에 할당하기 전에 다른 스크립트가 불러서 초기화 전에 요구한다면
    private GameObject interactProp;
    public GameObject Interactor
    {
        get
        {
            if (interactProp == null)
            {
                interactProp = interactor;
            }
            return interactProp;
        }
        private set { interactProp = value; }
    }

	// Start is called before the first frame update
	void Start()
    {
        qm = QuestManager.Instance;
		regionNameObj.SetActive(false);
        objectiveMainObj.SetActive(false);
        objectiveSubObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRegion(int idx)
    {
        StartCoroutine(ShowRegionNameCoroutine(idx));
    }


    public void ClearSub()
    {
        StartCoroutine(AchieveSubObjective());
    }

    /// 지역이름 UI 애니메이션
    /// 새 지역으로 들어왔다면 이 코루틴을 호출
    public IEnumerator ShowRegionNameCoroutine(int idx)
    {
        Debug.Log("Begin Coroutine");

        regionNameObj.SetActive(true);
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
		// 싱글턴에서 스트링으로 받아오자
        regionText.text = qm.RegionNames[idx];

        float elapsedTime = 0.0f;
        // 배경 페이드
        while (elapsedTime < regionFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / regionFadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;

        elapsedTime = 0.0f;

        // 지역이름 페이드
        while (elapsedTime < regionFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / regionFadeTime);
            yield return null;
        }
        regionText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSecondsRealtime(regionDurationTime);
        elapsedTime = 0.0f;

        // 페이드 아웃
        while (elapsedTime < regionFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / regionFadeTime);
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / regionFadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionNameObj.SetActive(false);

        // 첫 번째 목표는 여기서 띄우기
        if (idx == 0)
        {
            yield return new WaitForSeconds(1.0f);
            StartAppearMain(idx);
        }

        if (idx == 1)
        {
            yield return new WaitForSeconds(1.0f);
            StartAppearMain(3);
        }

        if (idx == 2)
        {
            yield return new WaitForSeconds(1.0f);
            StartAppearMain(6);
        }
    }

    /// 메인 목표 박스 UI 애니메이션
    /// QuestManager의 인덱스와 연동해서 팝업한다.
    /// 이 코루틴이 끝나면 서브 목표 코루틴까지 알아서 호출한다
    /// 중간에 코루틴 난입을 대비해서 한번 더 함수로 감싸는게 맞나
    public void StartAppearMain(int idx)
    {
        curCoroutine = StartCoroutine(AppearMainObjective(idx));
    }

    public IEnumerator AppearMainObjective(int idx)
    {
        Debug.Log("Show Main objective UI");
        // 퀘스트 시작
        qm.Questing();

        objectiveMainObj.SetActive(true);
        objectiveSubObj.SetActive(false);
        mainText.GetComponent<CanvasGroup>().alpha = 0.0f;
        mainText.text = qm.QuestLines[idx];
        subText.text = qm.QuestLines[idx];

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        // 목표 배경 페이드
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.Lerp(0, 1, elapsedTime / objFadeTime);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 1.0f;
        mainBackImage.transform.localScale = offset;

        mainText.gameObject.SetActive(true);
        elapsedTime = 0.0f;
        // 목표 텍스트 페이드
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / objFadeTime);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSecondsRealtime(objDurationTime);

        elapsedTime = 0.0f;
        // 텍스트 페이드아웃
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / objFadeTime);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        // 배경 페이드아웃
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.Lerp(1, 0, elapsedTime / objFadeTime);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 0.0f;
        mainBackImage.transform.localScale = offset;

        StartAppearSub(idx);
        objectiveMainObj.SetActive(false);
        curCoroutine = null;
    }

    /// 서브 목표 UI 띄우기
    /// 얘는 메인 목표가 나오면 무조건 호출된다.
    /// 단독으로 쓰일 일은 거의 없을 것 같다.
    public void StartAppearSub(int idx)
    {
        curCoroutine = StartCoroutine(AppearSubObjective(idx));
    }
    public IEnumerator AppearSubObjective(int idx)
    {
        Debug.Log("Show sub objective UI");
        objectiveSubObj.SetActive(true);
        subText.GetComponent<CanvasGroup>().alpha = 0.0f;
        subText.text = qm.QuestLines[idx];

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.SmoothStep(0, 1, elapsedTime / objFadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / objFadeTime);
            yield return null;
        }
        subText.GetComponent <CanvasGroup>().alpha = 1.0f;
    }

    /// 목표 달성하면 서브 UI 띠용하고 지우기
    public void Achieve()
    {
        StartCoroutine(AchieveSubObjective());
    }
    public IEnumerator AchieveSubObjective()
    {
        // curCoroutine은 이것을 위해서
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            objectiveMainObj.SetActive(false);
            objectiveSubObj.SetActive(true);
            subText.transform.localScale = Vector3.one;
            subText.GetComponent<CanvasGroup>().alpha = 1.0f;
            subBackImage.transform.localScale = Vector3.one;
            curCoroutine = null;
        }

        // 끝났다
        qm.QuestDone();

        Debug.Log("Achieve sub objective UI");
        Vector3 offset = new Vector3(1.3f, 1.3f, 1.3f);
        Vector3 sVec = offset;
        Vector3 eVec = new Vector3(1, 1, 1);
        subText.color = acvColor;

        float elapsedTime = 0.0f;
        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset = Vector3.Lerp(sVec, eVec, elapsedTime / 0.3f);
            subText.transform.localScale = offset;
            yield return null;
        }
        subText.transform.localScale = eVec;

        yield return new WaitForSecondsRealtime(2.0f);

        elapsedTime = 0.0f;
        while (elapsedTime < objFadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.SmoothStep(1, 0, elapsedTime / objFadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / objFadeTime);
            yield return null;
        }
        subText.GetComponent<CanvasGroup>().alpha = 0;
        subText.color = texColor;
        objectiveSubObj.SetActive(false);
    }

    /// 목표를 달성하지 않고 다음 목표가 나왔다면
    public IEnumerator FailSubObjective()
    {
        // curCoroutine은 이것을 위해서
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            objectiveMainObj.SetActive(false);
            objectiveSubObj.SetActive(true);
            subText.transform.localScale = Vector3.one;
            subText.GetComponent<CanvasGroup>().alpha = 1.0f;
            subBackImage.transform.localScale = Vector3.one;
            curCoroutine = null;
        }

        // 끝났다
        qm.QuestDone();

        Debug.Log("Failed to achieve sub objective UI");
        Vector3 offset = new Vector3(1.3f, 1.3f, 1.3f);
        Vector3 sVec = offset;
        Vector3 eVec = new Vector3(1, 1, 1);

        float failedTime = 0.6f;
        float elapsedTime = 0.0f;

        while (elapsedTime < failedTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset = Vector3.Lerp(eVec, sVec, elapsedTime / failedTime);
            subText.transform.localScale = offset;
            offset.y = Mathf.SmoothStep(1, 0, elapsedTime / failedTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / failedTime);
            yield return null;
        }
        subText.GetComponent<CanvasGroup>().alpha = 0;
        subText.color = texColor;
        subText.transform.localScale = eVec;
        objectiveSubObj.SetActive(false);

        yield return null;
    }
}
