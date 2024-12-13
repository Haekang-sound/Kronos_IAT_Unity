using UnityEngine;
using System.Collections;

/// <summary>
/// CanvasGroup의 투명도를 조절하여 페이드 인/아웃 효과를 제공합니다.
/// 특정 이벤트(ex: 보스 스페셜 이벤트)를 위한 페이드 시퀀스도 지원합니다.
/// </summary>

public class FadeEffector : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn(float fadeDuration)
    {
        if (_canvasGroup.alpha == 1)
            return;

        if (_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup 컴포넌트가 없습니다. 추가해주세요.");
            return;
        }

        StartCoroutine(FadeIn(fadeDuration));
    }

    public void StartFadeOut(float fadeDuration)
    {
        if (_canvasGroup.alpha == 0)
            return;

        if (_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup 컴포넌트가 없습니다. 추가해주세요.");
            return;
        }

        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator Fade(float fadeDuration, float targetAlpha)
    {
        float startTime = Time.unscaledTime;  // 시작 시간 설정
        float initialAlpha = _canvasGroup.alpha;  // 시작 알파 값 설정

        while (Time.unscaledTime < startTime + fadeDuration)
        {
            float t = (Time.unscaledTime - startTime) / fadeDuration;  // 현재 시간 계산
            _canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);  // 알파 값 보간
            yield return null;  // 다음 프레임까지 대기
        }

        _canvasGroup.alpha = targetAlpha;  // 목표 알파 값 설정
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        return Fade(fadeDuration, 1f);
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        return Fade(fadeDuration, 0f);
    }

    private IEnumerator ForBossSpecialEvent()
    {
        yield return FadeIn(2);

        yield return new WaitForSeconds(3);

        yield return FadeOut(2);
    }

    public void StartForBossSpecialEvent()
    {
        StartCoroutine(ForBossSpecialEvent());
    }
}
