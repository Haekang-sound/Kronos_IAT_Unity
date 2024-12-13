using UnityEngine;


/// <summary>
/// 스테이지 1 음악도 그냥 오브젝트에 붙여서 재생한다
/// </summary>
public class World1Music : MonoBehaviour
{
    public Sonity.SoundEvent bgm;
    private SoundManager se;

    void Start()
    {
        se = SoundManager.Instance;
        se.PlayBGM(bgm);
    }
}
