using System.Collections;
using TMPro;
using UnityEngine;
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

    public int sceneIdx = 0;
    public int objectiveIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        regionNameObj.SetActive(false);
        objectiveMainObj.SetActive(false);
        objectiveSubObj.SetActive(false);

        InitialzeJSON();

    }

    void InitialzeJSON()
    {
        Debug.Log("Initializing Json Loader");
        JasonSaveLoader loader = new JasonSaveLoader();
        loader.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRegion()
    {
        StartCoroutine(FadeRegionAlpha());
    }

    public void StartMain()
    {
        StartCoroutine(AppearMainObjective());
    }

    public void ClearSub()
    {
        StartCoroutine(AchieveSubObjective());
    }

    /// 지역이름 UI 애니메이션
    /// 새 지역으로 들어왔다면 이 코루틴을 호출
    public IEnumerator FadeRegionAlpha()
    {
        Debug.Log("Begin Coroutine");

        regionNameObj.SetActive(true);
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
        /// 제이슨 로드하고 텍스트 뽑기
        regionText.text = JasonSaveLoader.SceneTexts[sceneIdx].text;

        float elapsedTime = 0.0f;
        // 배경 페이드
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;

        //yield return new WaitForSeconds(1.0f);
        elapsedTime = 0.0f;

        // 지역이름 페이드
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(RegiondurationTime);
        elapsedTime = 0.0f;

        // 페이드 아웃
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / RegionfadeTime);
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionNameObj.SetActive(false);

        // 첫 번째 목표는 여기서 띄우기
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(AppearMainObjective());

        // 다음 지역이름으로 인덱스 올리기
        sceneIdx++;
    }

    /// 메인 목표 박스 UI 애니메이션
    /// 씬 인덱스에 맞는 액셀 시트의 텍스트를 자동으로 불러오며,
    /// 이 코루틴이 끝나면 서브 목표 코루틴까지 알아서 호출한다
    public IEnumerator AppearMainObjective()
    {
        Debug.Log("Show Main objective UI");

        objectiveMainObj.SetActive(true);
        mainText.GetComponent<CanvasGroup>().alpha = 0.0f;
        mainText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        // 목표 배경 페이드
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
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
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(ObjdurationTime);

        elapsedTime = 0.0f;
        // 텍스트 페이드아웃
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        // 배경 페이드아웃
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 0.0f;
        mainBackImage.transform.localScale = offset;

        StartCoroutine(AppearSubObjective());
        objectiveMainObj.SetActive(false);

    }

    /// 서브 목표 UI 띄우기
    /// 얘는 메인 목표가 나오면 무조건 호출된다.
    /// 단독으로 쓰일 일은 거의 없을 것 같다.
    public IEnumerator AppearSubObjective()
    {
        Debug.Log("Show sub objective UI");
        objectiveSubObj.SetActive(true);
        subText.GetComponent<CanvasGroup>().alpha = 0.0f;
        subText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.SmoothStep(0, 1, elapsedTime / ObjfadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            yield return null;
        }
        subText.GetComponent <CanvasGroup>().alpha = 1.0f;
    }

    /// 목표 달성하면 서브 UI 띠용하고 지우기
    /// 끝나고 자동으로 다음 메인 목표를 띄워야 할 수도 있다.
    public IEnumerator AchieveSubObjective()
    {
        Debug.Log("Achieve sub objective UI");
        Vector3 offset = new Vector3(1.3f, 1.3f, 1.3f);
        Vector3 sVec = offset;
        Vector3 eVec = new Vector3(1, 1, 1);
        subText.color = acvColor;

        float elapsedTime = 0.0f;
        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            offset = Vector3.Lerp(sVec, eVec, elapsedTime / 0.3f);
            subText.transform.localScale = offset;
            yield return null;
        }
        subText.transform.localScale = eVec;

        yield return new WaitForSeconds(2.0f);

        elapsedTime = 0.0f;
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.SmoothStep(1, 0, elapsedTime / ObjfadeTime);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        subText.GetComponent<CanvasGroup>().alpha = 0;
        subText.color = texColor;
        objectiveSubObj.SetActive(false);

        // 목표가 달성되었으니 목표 인덱스 증가
        objectiveIdx++;

        // TODO: 다음 메인 목표 띄우기. 필요하다면
        if (objectiveIdx == 1)
            StartCoroutine(AppearMainObjective());
    }

}
