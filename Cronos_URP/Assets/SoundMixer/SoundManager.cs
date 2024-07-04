using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sonity;

/// ���� �ε带 ����ϴ� �̱��� ���� �Ŵ���
public class SoundManager : MonoBehaviour
{
    // �̱��� & get
    static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject sm = new GameObject(typeof(EffectManager).Name);
                    instance = sm.AddComponent<SoundManager>();

                    DontDestroyOnLoad(sm);
                }
            }
            return instance;
        }
    }

    [SerializeField]
    Player player;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySFX("Attack_SE", player.transform);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayBGM("Demo_SE");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            StopBGM("Demo_SE");
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
        Debug.Log("���ҽ� ������ BGM �ε�");

        SoundEvent[] sfxs = Resources.LoadAll<SoundEvent>("Sound/Sfx");
        foreach (SoundEvent soundEvent in sfxs)
        {
            // ����Ʈ�� ��ųʸ��� ����
            SfxList.Add(soundEvent);
        }
        Debug.Log("���ҽ� ������ SFX �ε�");

    }

    void PlayBGM(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }

    void PlaySFX(string name, Transform transform)
    {
        foreach (SoundEvent se in SfxList)
        {
            if (se.name == name)
                se.Play(transform);
        }
    }

    // �����ε�
    void PlaySFX(string name)
    {
        foreach (SoundEvent se in SfxList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }

    void StopBGM(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.StopMusic();
        }
    }
}
