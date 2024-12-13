using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬 트리의 진행 상태를 시각적으로 표시하는 UI를 관리합니다. 
/// </summary>
public class ProgressBar : MonoBehaviour
{
    public readonly string defaultBarTextFormat = "{0:0.00} %";
    public readonly float defaultPercentage = 0.0f;
    public readonly float fillAmountDuration = 1.6f;

    [SerializeField] public Image backgroundImage;
    [SerializeField] public Image barImage;
    [SerializeField, CanBeNull] public TMP_Text barText;

    protected float percentage;

    public bool isAnimate;
    private bool isFilling;

    public void Reset()
    {
        barImage.fillAmount = defaultPercentage;
    }

    public virtual void Start()
    {
        percentage = defaultPercentage;
        UpdatePercentage(percentage);
    }

    public virtual void UpdatePercentage(float percentAmount)
    {
        if (!PercentInRange(percentAmount))
            return;

        percentage = percentAmount;
        SetBarFill();
        SetTextPercentage();
    }

    public float GetPercentage() => percentage;

    public void SetBarText(string text)
    {
        if (!(barText is null))
            barText.text = text;
    }

    private void SetBarFill()
    {
        if (isAnimate == true)
        {
            if (isFilling == false)
            {
                StartCoroutine(FillBarOverTime(percentage, fillAmountDuration)); // 1초 동안 fillAmount를 채움
            }
        }
        else
        {
            barImage.fillAmount = percentage;
        }
    }

    private IEnumerator FillBarOverTime(float targetPercentage, float duration)
    {
        float startFill = barImage.fillAmount;
        float endFill = ToDecimalPercentage(targetPercentage);
        float elapsedTime = 0f;

        isFilling = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            barImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / duration);
            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 정확한 값으로 설정
        barImage.fillAmount = endFill;

        isFilling = false;
    }

    public float ToDecimalPercentage(float percentage)
    {
        return percentage / 1f;
    }

    private void SetTextPercentage()
    {
        if (!(barText is null))
            barText.text = string.Format(defaultBarTextFormat, percentage);
    }
    public bool PercentInRange(float percentAmount)
    {
        return percentAmount >= 0f && percentAmount <= 1f;
    }
}
