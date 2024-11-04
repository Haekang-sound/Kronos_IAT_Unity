using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    // Get�ϴ� ������Ƽ
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

    [Tooltip("���� �̸��� ���̵�Ǵ� �ð��Դϴ�")]
    public float RegionfadeTime = 2.0f;
    [Tooltip("���� �̸��� �����Ǵ� �ð��Դϴ�")]
    public float RegiondurationTime = 3.0f;
    [Tooltip("��ǥ�� ���̵�Ǵ� �ð��Դϴ�")]
    public float ObjfadeTime = 1.0f;
    [Tooltip("��ǥ�� �����Ǵ� �ð��Դϴ�")]
    public float ObjdurationTime = 3.0f;

    // �ڷ�ƾ �� ȣ�� �� ���߱� ���ؼ�
    Coroutine curCoroutine;

    QuestManager qm;

    [SerializeField]
    private GameObject interactor;
    // lazy initialization
    // ������Ƽ�� �Ҵ��ϱ� ���� �ٸ� ��ũ��Ʈ�� �ҷ���
    // �ۺ� ������Ʈ�� �ٷ� �Ҵ��ϱ� �����ϱ�
    // �ʱ�ȭ ���� �䱸�Ѵٸ�, �ν����Ϳ��� �Ҵ��� ������ �ʱ�ȭ�ϰ� ����
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

    /// �����̸� UI �ִϸ��̼�
    /// �� �������� ���Դٸ� �� �ڷ�ƾ�� ȣ��
    public IEnumerator ShowRegionNameCoroutine(int idx)
    {
        Debug.Log("Begin Coroutine");

        regionNameObj.SetActive(true);
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
		/// ���̽� �ε��ϰ� �ؽ�Ʈ �̱�� ���� �����
        /// �� �̱��Ͽ��� ��Ʈ������ �޾ƿ���
		//regionText.text = JasonSaveLoader.SceneTexts[sceneIdx].text;
        regionText.text = qm.RegionNames[idx];

        float elapsedTime = 0.0f;
        // ��� ���̵�
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;

        //yield return new WaitForSeconds(1.0f);
        elapsedTime = 0.0f;

        // �����̸� ���̵�
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(RegiondurationTime);
        elapsedTime = 0.0f;

        // ���̵� �ƿ�
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

        // ù ��° ��ǥ�� ���⼭ ����
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

    /// ���� ��ǥ �ڽ� UI �ִϸ��̼�
    /// �� �ε����� �´� �׼� ��Ʈ�� �ؽ�Ʈ�� �ڵ����� �ҷ�����,

    /// �� �������̰�,
    /// �Ź����� QuestManager�� �ε����� �����ؼ� �˾��Ѵ�.
    /// �� �ڷ�ƾ�� ������ ���� ��ǥ �ڷ�ƾ���� �˾Ƽ� ȣ���Ѵ�
    /// �߰��� �ڷ�ƾ ������ ����ؼ� �ѹ� �� �Լ��� ���δ°� �³�
    public void StartAppearMain(int idx)
    {
        curCoroutine = StartCoroutine(AppearMainObjective(idx));
    }

    public IEnumerator AppearMainObjective(int idx)
    {
        Debug.Log("Show Main objective UI");
        // ����Ʈ ����
        qm.Questing();

        objectiveMainObj.SetActive(true);
        objectiveSubObj.SetActive(false);
        mainText.GetComponent<CanvasGroup>().alpha = 0.0f;
		//mainText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;
        mainText.text = qm.QuestLines[idx];
        subText.text = qm.QuestLines[idx];

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        // ��ǥ ��� ���̵�
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
        // ��ǥ �ؽ�Ʈ ���̵�
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(ObjdurationTime);

        elapsedTime = 0.0f;
        // �ؽ�Ʈ ���̵�ƿ�
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        // ��� ���̵�ƿ�
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

    /// ���� ��ǥ UI ����
    /// ��� ���� ��ǥ�� ������ ������ ȣ��ȴ�.
    /// �ܵ����� ���� ���� ���� ���� �� ����.
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

    /// ��ǥ �޼��ϸ� ���� UI ����ϰ� �����
    public void Achieve()
    {
        StartCoroutine(AchieveSubObjective());
    }
    public IEnumerator AchieveSubObjective()
    {
        // curCoroutine�� �̰��� ���ؼ�
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

        // ������
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

    /// ��ǥ �����ϸ� ���� UI ���������� �����
    
    public IEnumerator FailSubObjective()
    {
        // curCoroutine�� �̰��� ���ؼ�
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

        // ������
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
