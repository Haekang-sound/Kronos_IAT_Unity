using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sonity;

/// 사운드 로드를 담당하는 싱글턴 사운드 매니저
public class SoundManager : MonoBehaviour
{
    // 싱글턴 & get
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
                    GameObject sm = new GameObject(typeof(SoundManager).Name);
                    instance = sm.AddComponent<SoundManager>();

                    DontDestroyOnLoad(sm);
                }
            }
            return instance;
        }
    }

    [SerializeField]
    Player player;

    // 일단 BGM 리스트
    List<SoundEvent> BgmList = new List<SoundEvent>();
    List<SoundEvent> SfxList = new List<SoundEvent>();
    // Play를 id를 통해서 함수로 호출할거라면 딕셔너리가 괜찮을 것 같다
    Dictionary<int, SoundEvent> soundDictionary;

    // 캐싱한다면 여기서
    private void Awake()
    {
        instance = this;
        LoadSE();
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayBGM("Demo_SE");
        //SoundMixerNCamera.Instance.AdjustNewScene();
    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    StopBGM("Demo_SE");
        //}
    }

    // 리소스 폴더 내부의 SE를 한번에 로드
    void LoadSE()
    {
        SoundEvent[] bgms = Resources.LoadAll<SoundEvent>("Audio/Bgm");
        foreach (SoundEvent soundEvent in bgms)
        {
            // 리스트나 딕셔너리에 넣자
            BgmList.Add(soundEvent);
        }
        Debug.Log("리소스 폴더의 BGM 로드");

        SoundEvent[] sfxs = Resources.LoadAll<SoundEvent>("Audio/Sfx");
        foreach (SoundEvent soundEvent in sfxs)
        {
            // 리스트나 딕셔너리에 넣자
            SfxList.Add(soundEvent);
        }
        Debug.Log("리소스 폴더의 SFX 로드");

    }

    public void PlayBGM(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }

    public void PlayBGM(SoundEvent soev)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se == soev)
                se.PlayMusic();
        }
    }

    public void PlaySFX(string name, Transform transform)
    {
        foreach (SoundEvent se in SfxList)
        {
            if (se.name == name)
                se.Play(transform);
        }
    }

    // 오버로딩
    public void PlaySFX(string name)
    {
        foreach (SoundEvent se in SfxList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }


    public void StopBGM(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.StopMusic();
        }
    }

    public void StopSFX(string name)
    {
        foreach (SoundEvent se in SfxList)
        {
            if (se.name == name)
            {
                Debug.Log($"Found {se.name} in SfxList");
                se.StopMusic();
            }
        }
    }

    public void ButtonEnter()
    {
        PlaySFX("UI_MouseON_Effect_Sound_SE", transform);
    }

    public void ButtonClick()
    {
        PlaySFX("UI_MouseClick_Sound_SE", transform);
    }

    public void NextPage()
    {
        PlaySFX("Manual_Nextpage_Sound_SE", transform);
    }

    public void StartButtonSound()
    {
        PlaySFX("UI_GameStart_Button_Sound_SE", transform);
    }

	public void BossIntro()
	{
		PlayBGM("Vitus_intro_MP_SE");
	}

	public void BossLoop()
	{
		PlayBGM("chronos_boss_loop_SE");
	}

    public void GateOpenSound()
    {
        PlaySFX("Effect_StoneGate_Sound_SE", transform);
    }
}
