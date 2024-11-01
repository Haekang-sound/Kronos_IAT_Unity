using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSteam : MonoBehaviour
{
    SoundManager se;

    // Start is called before the first frame update
    void Start()
    {
        se = SoundManager.Instance;
        se.PlaySFX("Effect_Steam_1_Sound_SE", transform);
    }

}
