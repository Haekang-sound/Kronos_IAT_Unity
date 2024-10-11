using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	private List<string> lodingText;
	private static SceneLoader instance;
    // Get�ϴ� ������Ƽ
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                if (instance == null)
                {
                    GameObject sLoader = new GameObject(typeof(UIManager).Name);
                    instance = sLoader.AddComponent<SceneLoader>();

                    DontDestroyOnLoad(sLoader);
                }
            }
            return instance;
        }
    }
    public float fadeTime = 1.0f;
    public GameObject loadingObj;
    public Image progressArch;
    public Image loadingSpin;
    [SerializeField]
    TextMeshProUGUI loadingText;
	void Start()
	{}
    private void OnEnable()
    {
        
    }

    // �Լ��� �θ��Ŷ�� �̰� ����
    public void LoadScene(string indexName)
    {
        StartCoroutine(LoadSceneCoroutine(indexName));
    }

    // ���� �ε��ϴ� �ڷ�ƾ
    public IEnumerator LoadSceneCoroutine(string indexName)
    {
        progressArch.fillAmount = 0;
        
        AsyncOperation ao = SceneManager.LoadSceneAsync(indexName);
        ao.allowSceneActivation = false;

        if (Player.Instance != null)
        {
            Player.Instance.GetComponent<InputReader>().enabled = false;
        }

        // 5���� ��� �� �ϳ� �������� �̾Ƽ� �ؽ�Ʈ�� ������ �ϱ�
        List<int>rand = EffectManager.Instance.FisherYatesShuffles(5, 1);
		 loadingText.text = JasonSaveLoader.LoadingTexts[rand[0]].text;

		float progress = 0;

        while (!ao.isDone)
        {
            progress = Mathf.MoveTowards(progress, ao.progress, Time.deltaTime);
            progressArch.fillAmount = progress;

            loadingSpin.transform.Rotate(0, 0, 1f);

            if (progress >= 0.9f)
            {
                progressArch.fillAmount = 1;

                ao.allowSceneActivation = true;

                if (Player.Instance != null)
                {
                    Player.Instance.gameObject.GetComponent<InputReader>().enabled = true;
                }
            }

            yield return null;
        }
    }
}
