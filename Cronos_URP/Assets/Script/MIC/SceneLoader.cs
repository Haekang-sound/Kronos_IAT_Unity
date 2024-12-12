using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 씬을 로드하고, 로드하는 동안 로딩 게이지를 채워주는 클래스
/// </summary>
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

    [SerializeField]
    private TextMeshProUGUI loadingText;
    public float fadeTime = 1.0f;
    public Image progressArch;          // 로딩 바

    // 함수로 부를거라면 이걸 쓴다
    public void LoadScene(string indexName)
    {
        StartCoroutine(LoadSceneCoroutine(indexName));
    }

    // 로딩하고, 로딩 바도 채워준다
    public IEnumerator LoadSceneCoroutine(string indexName)
    {
        progressArch.fillAmount = 0;
        
        AsyncOperation ao = SceneManager.LoadSceneAsync(indexName);
        ao.allowSceneActivation = false;

        if (Player.Instance != null)
        {
            Player.Instance.GetComponent<InputReader>().enabled = false;
        }

		float progress = 0;

        while (!ao.isDone)
        {
            progress = Mathf.MoveTowards(progress, ao.progress, Time.deltaTime);
            progressArch.fillAmount = progress;


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
