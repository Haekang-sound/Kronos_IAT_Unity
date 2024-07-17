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

        // ���� �� ���̵� �� ȿ���� �����մϴ�.
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

        // ���� �� ���̵� �� ȿ���� �����մϴ�.
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
    private IEnumerator FadeOut(float fadeDuration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
