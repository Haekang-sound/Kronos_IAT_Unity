using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World1Music : MonoBehaviour
{
    public Sonity.SoundEvent bgm;
    SoundManager se;

    // Start is called before the first frame update
    void Start()
    {
        se = SoundManager.Instance;
        se.PlayBGM(bgm);
    }

}
