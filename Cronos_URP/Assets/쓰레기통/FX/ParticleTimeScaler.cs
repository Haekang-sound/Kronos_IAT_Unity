using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimeScaler : MonoBehaviour
{
    public ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        
    }

    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
