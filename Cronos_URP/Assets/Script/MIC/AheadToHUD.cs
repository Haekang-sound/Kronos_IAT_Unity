using System.Collections;
using UnityEngine;

public class AheadToHUD : MonoBehaviour
{
    public ParticleSystem ps;
    GameObject trigObj;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        trigObj = EffectManager.Instance.absorbBox;
        StartCoroutine(AheadToHUDCoroutine());
    }

    public IEnumerator AheadToHUDCoroutine()
    {
        yield return new WaitForSeconds(1);
        var externForces = ps.externalForces;
        var triggerMod = ps.trigger;
        externForces.enabled = true;
        triggerMod.SetCollider(0, trigObj.GetComponent<Collider>());
    }
}
