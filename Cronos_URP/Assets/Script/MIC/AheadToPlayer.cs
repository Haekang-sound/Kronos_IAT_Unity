using System.Collections;
using UnityEngine;

/// <summary>
/// 파티클이 플레이어를 향해 빨려들어가도록 하는 클래스
/// 이 클래스는 이펙트 프리팹에 붙어서 
/// 파티클 시스템이 외부 힘을 받을 수 있게 해준다.
/// </summary>
public class AheadToPlayer : MonoBehaviour
{
    public ParticleSystem ps;
    float burstNumber;
    GameObject trigObj;
    bool isTrigger;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        trigObj = EffectManager.Instance.absorbBox;
        RegisterBurst(burstNumber);
        StartCoroutine(AheadToPlayerCoroutine());
    }

    // 버스트되는 파티클 개수
    public float BurstNumber
    {
        get { return burstNumber; }
        set { burstNumber = value; }
    }

    // 파티클을 몇 개나 뿜을건지
    public void RegisterBurst(float bNum)
    {
        var emitModule = ps.emission;

        ParticleSystem.Burst burst = emitModule.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(bNum);

        emitModule.SetBurst(0, burst);
        Debug.Log("Burst number : " + bNum);
    }

    // 1초 기다린 뒤 외부 힘을 on함
    public IEnumerator AheadToPlayerCoroutine()
    {
        yield return new WaitForSeconds(1);
        var externForces = ps.externalForces;
        var triggerMod = ps.trigger;
        externForces.enabled = true;
        triggerMod.SetCollider(0, trigObj.GetComponent<Collider>());
    }

    private void OnParticleTrigger()
    {
        if (isTrigger)
        {
            return;
        }

        isTrigger = true;
        Debug.Log("Trigger particle!");
        Debug.Log("Gained TP : " + Player.Instance.TPGain() * burstNumber);
        Player.Instance.TP += Player.Instance.TPGain() * burstNumber;
        Debug.Log("Player current TP : " + Player.Instance.TP);

        // 파티클 트리거 되었을 때 TP HUD 초록색으로
        UI_TPCPHUD uiHud = UI_TPCPHUD.Instance;
        uiHud.ChangeGreen();
    }
}
