using System.Collections;
using UnityEngine;

/// <summary>
/// 페이딩 효과를 관리하는 클래스 입니다.
/// 씬 전환, 로딩, 게임 오버와 같은 상황에서 사용합니다
/// </summary>
public class ScreenFader : MonoBehaviour
{
    public enum FadeType
    {
        Black, 
        Loading, 
        GameOver,
    }

    public static ScreenFader Instance
    {
        get
        {
            if (s_instance != null)
            {
                return s_instance;
            }

            s_instance = FindObjectOfType<ScreenFader>();

            if (s_instance != null)
            {
                return s_instance;
            }

            Create();

            return s_instance;
        }
    }

    public static bool IsFading
    {
        get { return Instance.m_isFading; }
    }

    protected static ScreenFader s_instance;

    public CanvasGroup faderCanvasGroup;
    public CanvasGroup loadingCanvasGroup;
    public CanvasGroup gameOverCanvasGroup;
    public float fadeDuration = 1f;

    protected bool m_isFading;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void Create()
    {
        ScreenFader controllerPrefab = Resources.Load<ScreenFader>("ScreenFader");
        s_instance = Instantiate(controllerPrefab);
    }

    protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
    {
        m_isFading = true;
        canvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        canvasGroup.alpha = finalAlpha;
        canvasGroup.blocksRaycasts = false;
        m_isFading = false; 
    }

    public static void SetAlpha(float alpha, FadeType fadeType = FadeType.Black)
    {
        CanvasGroup canvasGroup;
        switch (fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.faderCanvasGroup;
                break;
            case FadeType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            case FadeType.Loading:
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }

        canvasGroup.alpha = alpha;
    }

    public static IEnumerator FadeSceneIn(FadeType fadeType = FadeType.Black)
    {
        CanvasGroup canvasGroup;
        switch (fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.faderCanvasGroup;
                break;
            case FadeType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            case FadeType.Loading:
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }

        yield return Instance.Fade(0f, canvasGroup);

        canvasGroup.gameObject.SetActive(false);
    }

    public static IEnumerator FadeSceneOut(FadeType fadeType = FadeType.Black)
    {
        CanvasGroup canvasGroup;
        switch (fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.faderCanvasGroup;
                break;
            case FadeType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            case FadeType.Loading:
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }

        canvasGroup.gameObject.SetActive(true);

        yield return Instance.Fade(1f, canvasGroup);
    }
}
