using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixer : MonoBehaviour
{
    // ���� ���� ����
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider sfxSlider;
    [SerializeField]
    TextMeshProUGUI masterVal;
    [SerializeField]
    TextMeshProUGUI bgmVal;
    [SerializeField]
    TextMeshProUGUI sfxVal;
    [SerializeField]
    Button applyButton;
    [SerializeField]
    GameObject confirmPopUp;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject pausePanel;
    CanvasGroup canvasGroup;

    public int dMas = 70;
    public int dBgm = 100;
    public int dSfx = 100;
    public int cMas;    // c ���, �����̴��� ������ ��
    public int cBgm;
    public int cSfx;
    public int aMas;    // a ���, ����Ǿ� ����� ��
    public int aBgm;
    public int aSfx;

    void Start()
    {
        if (bgmSlider == null)
            Debug.LogWarning("bgmSlider is not assigned.");
        else
            bgmSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("BgmVolume", AdjustVolume(x)));

        if (sfxSlider == null)
            Debug.LogWarning("sfxSlider is not assigned.");
        else
            sfxSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("SfxVolume", AdjustVolume(x)));

        if (masterSlider == null)
            Debug.LogWarning("masterSlider is not assigned.");
        else
            masterSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("MasterVolume", AdjustVolume(x)));

        aMas = dMas;
        aBgm = dBgm;
        aSfx = dSfx;
    }

    // Ȱ��ȭ�Ǹ� a��� ä���
    private void OnEnable()
    {
        aMas = cMas;
        aBgm = cBgm;
        aSfx = cSfx;
        confirmPopUp.SetActive(false);
        canvasGroup = optionPanel.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // c����� ����� ������ �����س���
        cMas = Mathf.RoundToInt(masterSlider.value * 100);
        cBgm = Mathf.RoundToInt(bgmSlider.value * 100);
        cSfx = Mathf.RoundToInt(sfxSlider.value * 100);

        // ���� ��ư�� Ȱ��ȭ�ϴ� ���ǹ�
        if (aMas != cMas || aBgm != cBgm || aSfx != cSfx)
        {
            applyButton.interactable = true;
        }
        else
        {
            applyButton.interactable = false;
        }

        // �����̴� ���� �ؽ�Ʈ�� �����ֱ�
        masterVal.text = ((masterSlider.value) * 100f).ToString("0") + ("%");
        bgmVal.text = ((bgmSlider.value) * 100f).ToString("0") + ("%");
        sfxVal.text = ((sfxSlider.value) * 100f).ToString("0") + ("%");
    }

    // �����ϸ� c����� a����� �ٲ۴�.
    public void ApplyValue()
    {
        aMas = cMas;
        aBgm = cBgm;
        aSfx = cSfx;
    }

    // Ȯ���ϸ� �ϴ� c����� a����� ������ Ȯ���Ѵ�.
    // �ٸ��ٸ� Ȯ�� �˾��� ����, �ƴ϶�� �ٷ� ����
    // Ȯ������ �� �������� ������ a����� �����ϰ� ����
    public void ConfirmValue()
    {
        // �ϳ��� �ٸ��� = �������� �ʰ� �����̴��� �ٲ� ���� �ִ�.
        // �ɼ� �г��� ����ĳ��Ʈ�� ���ƹ�����
        if (aMas != cMas || aBgm != cBgm || aSfx != cSfx)
        {
            confirmPopUp.SetActive(true);
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            FixValue();
            ExitPanel();
        }
    }

    // �ٲ�� ��������
    public void YesButton()
    {
        ApplyValue();
        FixValue();
        ExitPanel();
    }

    // ���� ���Ѵٸ� a����� �����ϰ� c����� a����� �ٲ���
    public void NoButton()
    {
        FixValue();
        cMas = aMas;
        cBgm = aBgm;
        cSfx = aSfx;
        ExitPanel();
    }

    // �⺻������ �����̴� ���� �ٲٱ� ������ ���Ѵ�
    public void ValueToDefault()
    {
        masterSlider.value = dMas / 100f;
        bgmSlider.value = dBgm / 100f;
        sfxSlider.value = dSfx / 100f;
    }

    // a����� �������� Ȯ���ϴ� �κ�
    void FixValue()
    {
        masterSlider.value = aMas / 100f;
        bgmSlider.value = aBgm / 100f;
        sfxSlider.value = aSfx / 100f;
    }

    public void ExitPanel()
    {
        // ����ĳ��Ʈ ��������
        canvasGroup.blocksRaycasts = true;
        optionPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // ���� �����̴��� �α� ���̴� ���� ������Ű����...
    public float AdjustVolume(float value)
    {
        // �ִ밪�� 1, �ּҰ��� 0.0001 -> 0�̸� ����� ���峪�� ������ �ö� 
        float clampVal = Mathf.Clamp(value, 0.0001f, 1f);
        // ���� �������� ���� ���������� ��ȯ
        float logVal = Mathf.Log10(clampVal);
        return logVal * 20f;
    }

}
