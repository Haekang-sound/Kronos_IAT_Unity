using UnityEngine;
using System.Collections;

public class FadeEffector : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn(float fadeDuration)
    {
        if (canvasGroup.alpha == 1)
            return;

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup ������Ʈ�� �����ϴ�. �߰����ּ���.");
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
            Debug.LogError("CanvasGroup ������Ʈ�� �����ϴ�. �߰����ּ���.");
            return;
        }

        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator Fade(float fadeDuration, float targetAlpha)
    {
        float startTime = Time.unscaledTime;  // ���� �ð� ����
        float initialAlpha = canvasGroup.alpha;  // ���� ���� �� ����

        while (Time.unscaledTime < startTime + fadeDuration)
        {
            float t = (Time.unscaledTime - startTime) / fadeDuration;  // ���� �ð� ���
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);  // ���� �� ����
            yield return null;  // ���� �����ӱ��� ���
        }

        canvasGroup.alpha = targetAlpha;  // ��ǥ ���� �� ����
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        return Fade(fadeDuration, 1f);
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        return Fade(fadeDuration, 0f);
    }
}
