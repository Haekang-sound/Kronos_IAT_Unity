using UnityEngine;
using System.Collections;

/// <summary>
/// CanvasGroup�� ������ �����Ͽ� ���̵� ��/�ƿ� ȿ���� �����մϴ�.
/// Ư�� �̺�Ʈ(ex: ���� ����� �̺�Ʈ)�� ���� ���̵� �������� �����մϴ�.
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
            Debug.LogError("CanvasGroup ������Ʈ�� �����ϴ�. �߰����ּ���.");
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
            Debug.LogError("CanvasGroup ������Ʈ�� �����ϴ�. �߰����ּ���.");
            return;
        }

        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator Fade(float fadeDuration, float targetAlpha)
    {
        float startTime = Time.unscaledTime;  // ���� �ð� ����
        float initialAlpha = _canvasGroup.alpha;  // ���� ���� �� ����

        while (Time.unscaledTime < startTime + fadeDuration)
        {
            float t = (Time.unscaledTime - startTime) / fadeDuration;  // ���� �ð� ���
            _canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);  // ���� �� ����
            yield return null;  // ���� �����ӱ��� ���
        }

        _canvasGroup.alpha = targetAlpha;  // ��ǥ ���� �� ����
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
