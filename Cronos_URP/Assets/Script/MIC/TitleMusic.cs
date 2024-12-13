using UnityEngine;


/// <summary>
/// 타이틀 씬 BGM은 그냥 오브젝트 하나에 이걸 붙여서 사용하고 있다
/// </summary>
public class TitleMusic : MonoBehaviour
{
    private SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = SoundManager.Instance;
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        Debug.Log("Play Title BGM");
        sm.PlayBGM("Title_Sound_Fix_SE");
    }
}
