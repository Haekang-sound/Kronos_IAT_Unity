using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public int sceneIdx = 0;
    public int objectiveIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        regionNameObj.SetActive(false);
        objectiveMainObj.SetActive(false);
        objectiveSubObj.SetActive(false);

        InitialzeJSON();

        
        //regionNames.Add("�ð��� �״� ��ΰ�");
        //regionNames.Add("������ �Ÿ�");
        //regionNames.Add("������ �ʴ� �ð��� ����");
    }

    void InitialzeJSON()
    {
        JasonSaveLoader loader = new JasonSaveLoader();
        loader.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �����̸� UI �ִϸ��̼�
    public IEnumerator FadeRegionAlpha()
    {
        Debug.Log("Begin Coroutine");

        regionNameObj.SetActive(true);
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
        /// ���̽� �ε��ϰ� �ؽ�Ʈ �̱�
        regionText.text = JasonSaveLoader.SceneTexts[sceneIdx].text;

        float elapsedTime = 0.0f;
        // ��� ���̵�
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;

        //yield return new WaitForSeconds(1.0f);
        elapsedTime = 0.0f;

        // �����̸� ���̵�
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(RegiondurationTime);
        elapsedTime = 0.0f;

        // ���̵� �ƿ�
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

        // ù ��° ��ǥ�� ���⼭ ����
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ShowObjectiveUI());

        // ���� �����̸����� �ε��� �ø���
        sceneIdx++;
    }

    // ���� ��ǥ �ڽ� UI �ִϸ��̼�
    public IEnumerator ShowObjectiveUI()
    {
        Debug.Log("Show Main objective UI");

        objectiveMainObj.SetActive(true);
        mainText.GetComponent<CanvasGroup>().alpha = 0.0f;
        mainText.text = JasonSaveLoader.QuestTexts[objectiveIdx].text;

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        // ��ǥ ��� ���̵�
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
        // ��ǥ �ؽ�Ʈ ���̵�
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(ObjdurationTime);

        elapsedTime = 0.0f;
        // �ؽ�Ʈ ���̵�ƿ�
        while (elapsedTime < ObjfadeTime)
        {
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / ObjfadeTime);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        // ��� ���̵�ƿ�
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

    // ���� ��ǥ UI ����
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

    // ��ǥ �޼��ϸ� ���� UI ����ϰ� �����
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

        // ��ǥ�� �޼��Ǿ����� ��ǥ �ε��� ����
        objectiveIdx++;

        // TODO: ���� ���� ��ǥ ����. �ʿ��ϴٸ�
    }

}
