using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

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
    GameObject regionNameObj;
    [SerializeField]
    Image regionImage;
    [SerializeField]
    TextMeshProUGUI regionText;
    [SerializeField]
    GameObject objectiveSubObj;
    [SerializeField]
    Image subBackImage;
    [SerializeField]
    TextMeshProUGUI subText;
    [SerializeField]
    GameObject objectiveMainObj;
    [SerializeField]
    Image mainBackImage;
    [SerializeField]
    TextMeshProUGUI mainText;

    Color texColor = new Color(0.96f, 0.96f, 0.96f);
    Color acvColor = new Color(0.431f, 0.886f, 0.357f);

    [Tooltip("지역 이름이 페이드되는 시간입니다")]
    public float RegionfadeTime = 2.0f;
    [Tooltip("지역 이름이 유지되는 시간입니다")]
    public float RegiondurationTime = 3.0f;
    [Tooltip("목표가 페이드되는 시간입니다")]
    public float ObjfadeTime = 1.0f;
    [Tooltip("목표가 유지되는 시간입니다")]
    public float ObjdurationTime = 3.0f;

    // 코루틴 중 호출 시 멈추기 위해서
    Coroutine curCoroutine;

    QuestManager qm;

    [SerializeField]
    private GameObject interactor;
    // lazy initialization
    // 프로퍼티에 할당하기 전에 다른 스크립트가 불러서
    // 퍼블릭 오브젝트를 바로 할당하기 싫으니까
    // 초기화 전에 요구한다면, 인스펙터에서 할당한 값으로 초기화하고 리턴
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
		/// 제이슨 로드하고 텍스트 뽑기는 서비스 종료다
        /// 걍 싱글턴에서 스트링으로 받아오자
		//regionText.text = JasonSaveLoader.SceneTexts[sceneIdx].text;
        regionText.text = qm.RegionNames[idx];

        float elapsedTime = 0.0f;
        // 배경 페이드
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;

        //yield return new WaitForSeconds(1.0f);
        elapsedTime = 0.0f;

        // 지역이름 페이드
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(RegiondurationTime);
        elapsedTime = 0.0f;

        // 페이드 아웃
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / RegionfadeTime);
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / RegionfadeTime);
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
    /// 씬 인덱스에 맞는 액셀 시트의 텍스트를 자동으로 불러오며,

    /// 은 구버전이고,
    /// 신버전은 QuestManager의 인덱스와 연동해서 팝업한다.
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
		//mainText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;
        mainText.text = qm.QuestLines[idx];
        subText.text = qm.QuestLines[idx];

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        // 목표 배경 페이드
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 1.0f;
        mainBackImage.transform.localScale = offset;

        mainText.gameObject.SetActive(true);
        elapsedTime = 0.0f;
        // 목표 텍스트 페이드
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(ObjdurationTime);

        elapsedTime = 0.0f;
        // 텍스트 페이드아웃
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        // 배경 페이드아웃
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 0.0f;
        mainBackImage.transform.localScale = offset;

        //StartCoroutine(AppearSubObjective(idx));
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
        //subText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;
        subText.text = qm.QuestLines[idx];

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.SmoothStep(0, 1, elapsedTime / ObjfadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
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

        yield return new WaitForSeconds(2.0f);

        elapsedTime = 0.0f;
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            offset.y = Mathf.SmoothStep(1, 0, elapsedTime / ObjfadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        subText.GetComponent<CanvasGroup>().alpha = 0;
        subText.color = texColor;
        objectiveSubObj.SetActive(false);
    }

    /// 목표 실패하면 서브 UI 빨간색으로 지우기
    
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
        //subText.color = Color.red;

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
