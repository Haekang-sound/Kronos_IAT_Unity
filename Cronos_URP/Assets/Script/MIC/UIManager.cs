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

    public float RegionfadeTime = 3.0f;
    public float RegiondurationTime = 5.0f;
    public float ObjfadeTime = 2.0f;
    public float ObjdurationTime = 3.0f;

    public List<string> regionNames;
    public List<string> objectives;

    // Start is called before the first frame update
    void Start()
    {
        regionNameObj.SetActive(false);
        objectiveMainObj.SetActive(false);
        objectiveSubObj.SetActive(false);

        regionNames.Add("�ð��� �״� ��ΰ�");
        regionNames.Add("������ �Ÿ�");
        regionNames.Add("������ �ʴ� �ð��� ����");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �����̸� UI �ִϸ��̼�
    public IEnumerator FadeRegionAlpha(int count)
    {
        Debug.Log("Begin Coroutine");

        regionNameObj.SetActive(true);
        regionImage.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionText.text = regionNames[count];

        float elapsedTime = 0.0f;
        // ��� ���̵�
        while (elapsedTime < RegionfadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionImage.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / RegionfadeTime);
            yield return null;
        }
        regionImage.GetComponent<CanvasGroup>().alpha = 1.0f;


        yield return new WaitForSeconds(1.0f);
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
    }

    // ���� ��ǥ UI ����
    public IEnumerator AppearSubObjective()
    {
        Debug.Log("Show sub objective UI");
        objectiveSubObj.SetActive(true);
        subText.GetComponent<CanvasGroup>().alpha = 0.0f;

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.SmoothStep(0, 1, elapsedTime / 1.0f);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / 1.0f);
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
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.SmoothStep(1, 0, elapsedTime / 1.0f);
            subBackImage.transform.localScale = offset;
            subText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / 1.0f);
            yield return null;
        }
        subText.GetComponent<CanvasGroup>().alpha = 0;
        subText.color = texColor;
        objectiveSubObj.SetActive(false);

        // TODO: ���� ���� ��ǥ ����. �ʿ��ϴٸ�
    }

    // ���� ��ǥ �ڽ� UI �ִϸ��̼�
    public IEnumerator ShowObjectiveUI()
    {
        Debug.Log("Show Main objective UI");

        objectiveMainObj.SetActive(true);
        mainText.GetComponent<CanvasGroup>().alpha = 0.0f;

        Vector3 offset = new Vector3(1, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.Lerp(0, 1, elapsedTime / 1.0f);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 1.0f;
        mainBackImage.transform.localScale = offset;

        mainText.gameObject.SetActive(true);
        elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / 1.0f);
            yield return null;
        }
        mainText.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(ObjdurationTime);

        elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            mainText.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / 1.0f);
            yield return null;
        }
        mainText.GetComponent <CanvasGroup>().alpha = 0.0f;
        
        elapsedTime = 0.0f;
        while (elapsedTime < 3.0f)
        {
            elapsedTime += Time.deltaTime;
            offset.y = Mathf.Lerp(1, 0, elapsedTime / 1.0f);
            mainBackImage.transform.localScale = offset;
            yield return null;
        }
        offset.y = 0.0f;
        mainBackImage.transform.localScale = offset;

        StartCoroutine(AppearSubObjective());
        objectiveMainObj.SetActive(false);

    }
}
