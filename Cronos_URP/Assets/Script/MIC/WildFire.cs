using UnityEngine;


/// <summary>
/// 불 프리팹에 붙이는 불 사운드 출력하는 클래스
/// </summary>
public class WildFire : MonoBehaviour
{
    private SoundManager se;

    void Start()
    {
        se = SoundManager.Instance;
        se.PlaySFX("Effect_Fire_1_Sound_SE", transform);
    }
}
