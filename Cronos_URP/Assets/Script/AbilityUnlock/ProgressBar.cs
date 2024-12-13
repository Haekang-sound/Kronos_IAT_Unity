using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų Ʈ���� ���� ���¸� �ð������� ǥ���ϴ� UI�� �����մϴ�. 
/// </summary>
public class ProgressBar : MonoBehaviour
{
    public const string DefaultBarTextFormat = "{0:0.00} %";
    public const float DefaultPercentage = 0.0f;

    [SerializeField] public Image backgroundImage;
    [SerializeField] public Image barImage;
    [SerializeField, CanBeNull] public TMP_Text barText;

    protected float percentage;

    public bool isAnimate;
    private bool isFilling;

    public void Reset()
    {
        barImage.fillAmount = 0f;
    }

    public virtual void Start()
    {
        percentage = DefaultPercentage;
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
                StartCoroutine(FillBarOverTime(percentage, 1.6f)); // 1�� ���� fillAmount�� ä��
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
            yield return null; // ���� �����ӱ��� ���
        }

        // ���������� ��Ȯ�� ������ ����
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
            barText.text = string.Format(DefaultBarTextFormat, percentage);
    }
    public bool PercentInRange(float percentAmount)
    {
        return percentAmount >= 0f && percentAmount <= 1f;
    }
}
