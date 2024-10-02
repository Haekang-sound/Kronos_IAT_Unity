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
    // Get하는 프로퍼티
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
	{
		lodingText.Add("크로노스가 창조한 크로노티카는 모든 것이 시간으로 이루어진 세계입니다. 이곳에서는 시간 그 자체가 생명이며, 권력의 원천입니다. 땅과 하늘, 모든 존재가 시간 속에서 태어나고 소멸하며, 그 흐름을 지배하는 자가 세상을 통제합니다.");
		lodingText.Add("템포리움은 크로노티카에서 가장 거대한 제국으로, 시간의 힘을 통해 찬란한 발전을 이루어냈습니다. 시간의 흐름을 지배하는 자들이 이곳을 운영하며, 그들의 기술과 지식은 제국을 번영의 정상으로 이끌었습니다. 하지만 그 번영 뒤에는 수많은 이들의 시간이 희생되고 있습니다.");
		lodingText.Add("시간의 그림자 빈민가는 크로노티카의 가장 어두운 구석입니다. 이곳에서는 시간이 더 이상 희망이 아닌, 생존을 위한 무거운 짐입니다. 잃어버린 시간 속에서 살아가는 이들은 오늘도 흐릿한 미래를 꿈꾸지만, 그들에게 주어진 시간은 너무나도 짧습니다.");
		lodingText.Add("질서의 거리는 크로노티카에서 가장 안정된 지역으로, 시간의 흐름마저도 규칙에 따라 정돈되어 있습니다. 이곳에서는 모든 것이 정해진 계획대로 움직이며, 혼란 대신 완벽한 질서가 지배합니다. ");
		lodingText.Add("멈추지 않는 시간의 광장은 크로노티카의 심장부로, 끊임없이 흐르는 시간이 모습을 드러내는 곳입니다. 바삐 움직이는 사람들과 거대한 분수는 멈추지 않는 시간을 상징합니다. 이곳에서 모든 것은 시간의 흐름에 따라 움직입니다.");
	}
    private void OnEnable()
    {
        
    }

    // 함수로 부를거라면 이걸 쓴다
    public void LoadScene(string indexName)
    {
        StartCoroutine(LoadSceneCoroutine(indexName));
    }

    // 씬을 로드하는 코루틴
    public IEnumerator LoadSceneCoroutine(string indexName)
    {
        progressArch.fillAmount = 0;
        
        AsyncOperation ao = SceneManager.LoadSceneAsync(indexName);
        ao.allowSceneActivation = false;

        if (Player.Instance != null)
        {
            Player.Instance.GetComponent<InputReader>().enabled = false;
        }

        // 5개의 요소 중 하나 랜덤으로 뽑아서 텍스트로 나오게 하기
        List<int>rand = EffectManager.Instance.FisherYatesShuffles(5, 1);
		// loadingText.text = JasonSaveLoader.LoadingTexts[rand[0]].text;
		//loadingText.text = lodingText[rand[0]];

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
