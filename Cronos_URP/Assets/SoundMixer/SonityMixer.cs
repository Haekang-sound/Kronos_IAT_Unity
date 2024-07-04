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


    void Start()
    {
        //masterSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("MasterVolume", AdjustVolume(x)));
        //bgmSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("BgmVolume", AdjustVolume(x)));
        //sfxSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("SfxVolume", AdjustVolume(x)));
    }

    // ���� �߿��� ������Ʈ���� ���ҽ��� �θ��� �͵� ������
    void Update()
    {
        
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

}
