using Sonity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildFire : MonoBehaviour
{
    SoundManager se;

    // Start is called before the first frame update
    void Start()
    {
        se = SoundManager.Instance;
        se.PlaySFX("Effect_Fire_1_Sound_SE", transform);
    }
}
