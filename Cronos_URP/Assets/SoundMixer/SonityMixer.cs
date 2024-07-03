using UnityEngine;
using Sonity;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SonityMixer : MonoBehaviour
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
    Player player;
    [SerializeField]
    Transform camera;


    // �ϴ� BGM ����Ʈ
    List<SoundEvent> BgmList = new List<SoundEvent>();
    List<SoundEvent> SfxList = new List<SoundEvent>();
    // Play�� id�� ���ؼ� �Լ��� ȣ���ҰŶ�� ��ųʸ��� ������ �� ����
    Dictionary<int, SoundEvent> soundDictionary;

    // ĳ���Ѵٸ� ���⼭
    private void Awake()
    {
        LoadSE();
    }


    void Start()
    {
        //masterSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("MasterVolume", AdjustVolume(x)));
        //bgmSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("BgmVolume", AdjustVolume(x)));
        //sfxSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("SfxVolume", AdjustVolume(x)));
    }

    // ���� �߿��� ������Ʈ���� ���ҽ��� �θ��� �͵� ������
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var se = Resources.Load<SoundEvent>("Sound/Bgm/Demo_SE");
            se.PlayMusic();
            Debug.Log("���� �����");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySound("Demo_SE");
        }
    }

    // ���ҽ� ���� ������ SE�� �ѹ��� �ε�
    void LoadSE()
    {
        SoundEvent[] bgms = Resources.LoadAll<SoundEvent>("Sound/Bgm");
        foreach (SoundEvent soundEvent in bgms)
        {
            // ����Ʈ�� ��ųʸ��� ����
            BgmList.Add(soundEvent);
        }
        Debug.Log("���ҽ� ������ ���� �ε�");
    }

    // ���� �����̴��� �α� ���̴� ���� ������Ű����...
    public float AdjustVolume(float value)
    {
        // �ִ밪�� 1, �ּҰ��� 0.0001 -> 0�̸� ����� ���峪�� ������ �ö� 
        float clampVal = Mathf.Clamp(value, 0.0001f, 1f);
        // ���� �������� ���� ���������� ��ȯ
        float logVal = Mathf.Log10(clampVal) * 20f;
        return logVal;
    }

    void PlaySound(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }
}
