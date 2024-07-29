using System;
using UnityEngine;

public class ShakeTest : MonoBehaviour
{
    [SerializeField]
    Player player;
    [Range(0, 10)]
    public float shakeStrength = 1.0f;

    ImpulseCam impulseCam;
    public float interval = 1.0f;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        impulseCam = ImpulseCam.Instance;
        timer = interval;    
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Shake(shakeStrength);
            timer = interval;
        }

    }

    void Shake(float ss)
    {
        impulseCam.Shake(ss);
    }
}
