using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerNCamera : MonoBehaviour
{
    // 볼륨 조절 관련
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
    public int cMas;    // c 밸류, 슬라이더로 조절될 값
    public int cBgm;
    public int cSfx;
    public int aMas;    // a 밸류, 적용되어 저장된 값
    public int aBgm;
    public int aSfx;

    // 카메라 조절 관련
    [SerializeField]
    CinemachineVirtualCamera virCam;
    [SerializeField]
    Slider camSlider;

    CinemachinePOV pov;
    public int dCamX = 50;
    public int dCamY = 80;
    public Vector2 cSpeed;     // c밸류, 슬라이더로 조절될 값
    public Vector2 aSpeed;     // a밸류, 적용될 최종값

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
            Debug.Log("pov를 찾았습니다");

        aSpeed.x = dCamX / 5;
        aSpeed.y = dCamY / 5;
    }

    // 활성화되면 a밸류 채우기
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
        // c밸류에 변경된 값들을 저장해놓자
        cMas = Mathf.RoundToInt(masterSlider.value * 100);
        cBgm = Mathf.RoundToInt(bgmSlider.value * 100);
        cSfx = Mathf.RoundToInt(sfxSlider.value * 100);

        // 카메라도 c밸류에 슬라이더 저장
        AdjustSensitivity(camSlider.value);

        // 적용 버튼을 활성화하는 조건문
        if (aMas != cMas || aBgm != cBgm || aSfx != cSfx || aSpeed != cSpeed)
        {
            applyButton.interactable = true;
        }
        else
        {
            applyButton.interactable = false;
        }

        // 슬라이더 값을 텍스트로 보여주기
        masVal.text = ((masterSlider.value) * 100f).ToString("0") + ("%");
        bgmVal.text = ((bgmSlider.value) * 100f).ToString("0") + ("%");
        sfxVal.text = ((sfxSlider.value) * 100f).ToString("0") + ("%");
        camVal.text = ((camSlider.value) * 100f).ToString("0") + ("%");
    }

    // 적용하면 c밸류를 a밸류로 바꾼다.
    public void ApplyValue()
    {
        aMas = cMas;
        aBgm = cBgm;
        aSfx = cSfx;

        aSpeed = cSpeed;
    }

    // 확인하면 일단 c밸류가 a밸류와 같은지 확인한다.
    // 다르다면 확인 팝업을 띄우고, 아니라면 바로 종료
    // 확인했을 때 적용하지 않으면 a밸류로 적용하고 종료
    public void ConfirmValue()
    {
        // 하나라도 다르다 = 적용하지 않고 슬라이더를 바꾼 값이 있다.
        // 옵션 패널의 레이캐스트를 막아버리자
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

    // 바뀐걸 적용하자
    public void YesButton()
    {
        ApplyValue();
        FixValue();
        ExitPanel();
    }

    // 적용 안한다면 a밸류를 적용하고 c밸류를 a밸류로 바꾸자
    public void NoButton()
    {
        FixValue();
        cMas = aMas;
        cBgm = aBgm;
        cSfx = aSfx;
        cSpeed = aSpeed;
        ExitPanel();
    }

    // 기본값으로 슬라이더 값을 바꾸기 적용은 안한다
    public void ValueToDefault()
    {
        masterSlider.value = dMas / 100f;
        bgmSlider.value = dBgm / 100f;
        sfxSlider.value = dSfx / 100f;
        camSlider.value = dCamX / 100f;
    }

    // a밸류로 최종값을 확정하는 부분
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
        // 레이캐스트 돌려놓고가
        canvasGroup.blocksRaycasts = true;
        optionPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseMenu.isOption = false;
    }

    // 선형 슬라이더에 로그 페이더 값을 변형시키려면...
    public float AdjustVolume(float value)
    {
        // 최대값은 1, 최소값은 0.0001 -> 0이면 계산이 고장나고 볼륨이 올라감 
        float clampVal = Mathf.Clamp(value, 0.0001f, 1f);
        // 선형 보간값을 감쇠 보간값으로 변환
        float logVal = Mathf.Log10(clampVal);
        return logVal * 20f;
    }

    // 슬라이더 값을 pov 에임 스피드에 적합한 값으로 바꾼다
    public void AdjustSensitivity(float value)
    {
        cSpeed.x = Mathf.RoundToInt((value / 5) * 100);
        cSpeed.y = Mathf.RoundToInt((value / 5) * 160);
    }
}
