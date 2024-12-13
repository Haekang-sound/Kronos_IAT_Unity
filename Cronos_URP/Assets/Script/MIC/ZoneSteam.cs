using UnityEngine;


/// <summary>
/// 증기에 붙일 증기 사운드 출력 클래스
/// </summary>
public class ZoneSteam : MonoBehaviour
{
    private SoundManager se;

    void Start()
    {
        se = SoundManager.Instance;
        se.PlaySFX("Effect_Steam_1_Sound_SE", transform);
    }
}
