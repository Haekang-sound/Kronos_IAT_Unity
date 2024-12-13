using UnityEngine;

public class BossEvent : MonoBehaviour
{
    SoundManager sm;

    void Start()
    {
        sm = SoundManager.Instance;
    }

    public void BossFoot()
    {
        if (sm != null)
            sm.PlaySFX("Boss_Walk_1_Sound_SE", transform);
    }
}
