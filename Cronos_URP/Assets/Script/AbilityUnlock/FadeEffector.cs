using UnityEngine;
using System.Collections;

public class FadeEffector : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn(float fadeDuration)
    {
        if (canvasGroup.alpha == 1)
            return;

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup 컴포넌트가 없습니다. 추가해주세요.");
            return;
        }

        StartCoroutine(FadeIn(fadeDuration));
    }

    public void StartFadeOut(float fadeDuration)
    {
        if (canvasGroup.alpha == 0)
            return;

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup 컴포넌트가 없습니다. 추가해주세요.");
            return;
        }

        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator Fade(float fadeDuration, float targetAlpha)
    {
        float startTime = Time.unscaledTime;  // 시작 시간 설정
        float initialAlpha = canvasGroup.alpha;  // 시작 알파 값 설정

        while (Time.unscaledTime < startTime + fadeDuration)
        {
            float t = (Time.unscaledTime - startTime) / fadeDuration;  // 현재 시간 계산
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);  // 알파 값 보간
            yield return null;  // 다음 프레임까지 대기
        }

        canvasGroup.alpha = targetAlpha;  // 목표 알파 값 설정
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        return Fade(fadeDuration, 1);
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        return Fade(fadeDuration, 0);
    }
}
