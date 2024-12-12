using System.Collections;
using UnityEngine;


/// <summary>
/// tab키로 우측의 조작 UI를 온/오프하는 클래스
/// 온/오프 도중에 인풋이 들어와도 제대로 작동할 수 있게
/// 현재 코루틴을 체크하여 중단시키고 새 코루틴으로 호출한다.
/// </summary>
public class KeyGuidance : MonoBehaviour
{
    [SerializeField]
    private GameObject keyGuide;

    private float fadePos = 876.0f;

    private float fadeTime = 0.5f;
    private bool nowShowing;
    private Coroutine curCoroutine;

    public Vector2 showPos;
    public Vector2 hidePos;

    // Start is called before the first frame update
    void Start()
    {
        showPos = keyGuide.GetComponent<RectTransform>().anchoredPosition;
        hidePos = new Vector2(fadePos, showPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && PauseManager.Instance.escAvailable)
        {
            if (!nowShowing)
                curCoroutine = StartCoroutine(ShowHUD());
            else
                curCoroutine = StartCoroutine(HideHUD());
        }
    }

    public void ShowGuide()
    {
        if (curCoroutine != null)
            StopCoroutine(curCoroutine);
        curCoroutine = StartCoroutine(ShowHUD());
        Debug.Log("Show key guide");
    }

    public void HideGuide()
    {
        if (curCoroutine != null)
            StopCoroutine(curCoroutine);
        curCoroutine = StartCoroutine(HideHUD());
        Debug.Log("Hide key guide");
    }

    // hide하는 중에 호출된다면, 현재 진행중인 hide코루틴을 중단하고 위치를 보정해서 show한다
    IEnumerator ShowHUD()
    {
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            keyGuide.GetComponent<CanvasGroup>().alpha = 0.0f;
            keyGuide.GetComponent<RectTransform>().anchoredPosition = hidePos;
            curCoroutine = null;
        }
        nowShowing = true;

        keyGuide.SetActive(true);
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime);  // Lerp 비율 제한
            keyGuide.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            keyGuide.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(hidePos, showPos, t);
            yield return null;
        }
        keyGuide.GetComponent<CanvasGroup>().alpha = 1.0f;
        curCoroutine = null;
    }

    // 마찬가지로 show 하는 도중 호출된다면 원래 갔어야 할 위치로 바로 보낸 후 hide한다.
    IEnumerator HideHUD()
    {
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            keyGuide.GetComponent<CanvasGroup>().alpha = 1.0f;
            keyGuide.GetComponent<RectTransform>().anchoredPosition = showPos;
            curCoroutine = null;
        }
        nowShowing = false;

        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeTime);  // Lerp 비율 제한
            keyGuide.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            keyGuide.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(showPos, hidePos, t);
            yield return null;
        }
        keyGuide.GetComponent<CanvasGroup>().alpha = 0.0f;
        keyGuide.SetActive(false);
        curCoroutine = null;
    }
}
