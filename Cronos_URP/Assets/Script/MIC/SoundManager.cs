using System.Collections.Generic;
using UnityEngine;
using Sonity;


///<summary>
/// 사운드 로드및 재생을 담당하는 사운드 매니저 클래스
/// 소니티에서 자체적으로 사운드 매니저를 가지고 있지만, 스크립트로 관리하기 위해 하나 더 만들었다
/// </summary>
public class SoundManager : MonoBehaviour
{
    // 싱글턴 & get
    private static SoundManager instance;
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
    private Player player;

    // 일단 BGM 리스트
    private List<SoundEvent> bgmList = new List<SoundEvent>();
    private List<SoundEvent> sfxList = new List<SoundEvent>();
    // Play를 id를 통해서 함수로 호출할거라면 딕셔너리가 괜찮을 것 같다
    private Dictionary<int, SoundEvent> soundDictionary;

    // 캐싱한다면 여기서
    private void Awake()
    {
        instance = this;
        LoadSE();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }

    // 리소스 폴더 내부의 SE를 한번에 로드
    void LoadSE()
    {
        SoundEvent[] bgms = Resources.LoadAll<SoundEvent>("Audio/Bgm");
        foreach (SoundEvent soundEvent in bgms)
        {
            // 리스트나 딕셔너리에 넣자
            bgmList.Add(soundEvent);
        }
        Debug.Log("리소스 폴더의 BGM 로드");

        SoundEvent[] sfxs = Resources.LoadAll<SoundEvent>("Audio/Sfx");
        foreach (SoundEvent soundEvent in sfxs)
        {
            // 리스트나 딕셔너리에 넣자
            sfxList.Add(soundEvent);
        }
        Debug.Log("리소스 폴더의 SFX 로드");

    }

    public void PlayBGM(string name)
    {
        foreach (SoundEvent se in bgmList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }

    public void PlayBGM(SoundEvent soev)
    {
        foreach (SoundEvent se in bgmList)
        {
            if (se == soev)
                se.PlayMusic();
        }
    }

    public void PlaySFX(string name, Transform transform)
    {
        foreach (SoundEvent se in sfxList)
        {
            if (se.name == name)
                se.Play(transform);
        }
    }

    // 오버로딩
    public void PlaySFX(string name)
    {
        foreach (SoundEvent se in sfxList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }


    public void StopBGM(string name)
    {
        foreach (SoundEvent se in bgmList)
        {
            if (se.name == name)
                se.StopMusic();
        }
    }

    public void StopSFX(string name)
    {
        foreach (SoundEvent se in sfxList)
        {
            if (se.name == name)
            {
                Debug.Log($"Found {se.name} in SfxList");
                se.StopMusic();
            }
        }
    }

    // 타임라인이나 키프레임에서 호출하기 위해서 별도로 구현했다
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
