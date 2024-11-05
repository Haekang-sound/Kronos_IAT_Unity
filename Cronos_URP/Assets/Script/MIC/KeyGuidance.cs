using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGuidance : MonoBehaviour
{
    [SerializeField]
    GameObject keyGuide;

    //public float leftPos = 1647.0f;
    float fadePos = 876.0f;

    float fadeTime = 0.5f;
    public bool nowShowing;
    Coroutine curCoroutine;

    public Vector2 showPos;
    public Vector2 hidePos;

    // Start is called before the first frame update
    void Start()
    {
        //nowShowing = true;
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

    // 조작가이드를 show 하는 코루틴
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

    // 조작가이드를 hide하는 코루틴
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
