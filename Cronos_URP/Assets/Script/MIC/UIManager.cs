using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    GameObject regionNameUI;
    [SerializeField]
    TextMeshProUGUI regionName;

    public float fadeTime = 3.0f;
    public float durationTime = 5.0f;

    public List<string> regionNames;

    // Start is called before the first frame update
    void Start()
    {
        regionNameUI.SetActive(false);

        regionNames.Add("림그레이브");
        regionNames.Add("재의 묘소");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeAlpha(int count)
    {
        Debug.Log("Begin Coroutine");

        regionNameUI.SetActive(true);
        regionNameUI.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionName.text = regionNames[count];

        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionNameUI.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            yield return null;
        }
        regionNameUI.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(durationTime);
        elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            regionNameUI.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            yield return null;
        }
        regionNameUI.GetComponent<CanvasGroup>().alpha = 0.0f;
        regionNameUI.SetActive(false);
    }
}
