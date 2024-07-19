using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerNCamera : MonoBehaviour
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
    TextMeshProUGUI masVal;
    [SerializeField]
    TextMeshProUGUI bgmVal;
    [SerializeField]
    TextMeshProUGUI sfxVal;
    [SerializeField]
    TextMeshProUGUI camVal;
    [SerializeField]
    Button applyButton;
    [SerializeField]
    GameObject confirmPopUp;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    PauseMenu pauseMenu;
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

    // ī�޶� ���� ����
    [SerializeField]
    CinemachineVirtualCamera virCam;
    [SerializeField]
    Slider camSlider;

    CinemachinePOV pov;
    public int dCamX = 50;
    public int dCamY = 80;
    public Vector2 cSpeed;     // c���, �����̴��� ������ ��
    public Vector2 aSpeed;     // a���, ����� ������

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

        if (camSlider == null)
            Debug.LogWarning("camSlider is not assigned");
        else
            pov = virCam.GetCinemachineComponent<CinemachinePOV>();

        if (pov != null)
            Debug.Log("pov�� ã�ҽ��ϴ�");

        aSpeed.x = dCamX / 5;
        aSpeed.y = dCamY / 5;
    }

    // Ȱ��ȭ�Ǹ� a��� ä���
    private void OnEnable()
    {
        aMas = cMas;
        aBgm = cBgm;
        aSfx = cSfx;
        aSpeed = cSpeed;
        confirmPopUp.SetActive(false);
        canvasGroup = optionPanel.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // c����� ����� ������ �����س���
        cMas = Mathf.RoundToInt(masterSlider.value * 100);
        cBgm = Mathf.RoundToInt(bgmSlider.value * 100);
        cSfx = Mathf.RoundToInt(sfxSlider.value * 100);

        // ī�޶� c����� �����̴� ����
        AdjustSensitivity(camSlider.value);

        // ���� ��ư�� Ȱ��ȭ�ϴ� ���ǹ�
        if (aMas != cMas || aBgm != cBgm || aSfx != cSfx || aSpeed != cSpeed)
        {
            applyButton.interactable = true;
        }
        else
        {
            applyButton.interactable = false;
        }

        // �����̴� ���� �ؽ�Ʈ�� �����ֱ�
        masVal.text = ((masterSlider.value) * 100f).ToString("0") + ("%");
        bgmVal.text = ((bgmSlider.value) * 100f).ToString("0") + ("%");
        sfxVal.text = ((sfxSlider.value) * 100f).ToString("0") + ("%");
        camVal.text = ((camSlider.value) * 100f).ToString("0") + ("%");
    }

    // �����ϸ� c����� a����� �ٲ۴�.
    public void ApplyValue()
    {
        aMas = cMas;
        aBgm = cBgm;
        aSfx = cSfx;

        aSpeed = cSpeed;
    }

    // Ȯ���ϸ� �ϴ� c����� a����� ������ Ȯ���Ѵ�.
    // �ٸ��ٸ� Ȯ�� �˾��� ����, �ƴ϶�� �ٷ� ����
    // Ȯ������ �� �������� ������ a����� �����ϰ� ����
    public void ConfirmValue()
    {
        // �ϳ��� �ٸ��� = �������� �ʰ� �����̴��� �ٲ� ���� �ִ�.
        // �ɼ� �г��� ����ĳ��Ʈ�� ���ƹ�����
        if (aMas != cMas || aBgm != cBgm || aSfx != cSfx || aSpeed != cSpeed)
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
        cSpeed = aSpeed;
        ExitPanel();
    }

    // �⺻������ �����̴� ���� �ٲٱ� ������ ���Ѵ�
    public void ValueToDefault()
    {
        masterSlider.value = dMas / 100f;
        bgmSlider.value = dBgm / 100f;
        sfxSlider.value = dSfx / 100f;
        camSlider.value = dCamX / 100f;
    }

    // a����� �������� Ȯ���ϴ� �κ�
    void FixValue()
    {
        masterSlider.value = aMas / 100f;
        bgmSlider.value = aBgm / 100f;
        sfxSlider.value = aSfx / 100f;
        camSlider.value = aSpeed.x * 5 / 100f;
        pov.m_VerticalAxis.m_MaxSpeed = aSpeed.x / 100f;
        pov.m_HorizontalAxis.m_MaxSpeed = aSpeed.y / 100f;
    }

    public void ExitPanel()
    {
        // ����ĳ��Ʈ ��������
        canvasGroup.blocksRaycasts = true;
        optionPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseMenu.isOption = false;
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

    // �����̴� ���� pov ���� ���ǵ忡 ������ ������ �ٲ۴�
    public void AdjustSensitivity(float value)
    {
        cSpeed.x = Mathf.RoundToInt((value / 5) * 100);
        cSpeed.y = Mathf.RoundToInt((value / 5) * 160);
    }
}
