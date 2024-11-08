using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadToHUD : MonoBehaviour
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
        StartCoroutine(AheadToHUDCoroutine());
    }

    // 버스트되는 파티클 개수를 정하는 프로퍼티
    public float BurstNumber
    {
        get { return burstNumber; }
        set { burstNumber = value; }
    }

    // 변수 값을 파티클 시스템의 버스트 카운트로 등록함
    public void RegisterBurst(float bNum)
    {
        var emitModule = ps.emission;

        ParticleSystem.Burst burst = emitModule.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(bNum);

        emitModule.SetBurst(0, burst);
        Debug.Log("Burst number : " + bNum);
    }

    // 1초 기다린 뒤 파티클 시스템의 외부 힘을 on함
    public IEnumerator AheadToHUDCoroutine()
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

        // 파티클 배열 생성 및 가져오기
        //ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        //int count = ps.GetParticles(particles);

        //for (int i = 0; i < count; i++)
        //{
        //    // 특정 조건에 따라 Kill을 결정합니다.
        //    // 예를 들어, 트리거된 파티클의 레이어가 targetLayer와 일치할 때 Kill합니다.
        //    Collider collider = Physics.OverlapSphere(particles[i].position, 0.1f)[0];
        //    if (collider != null && ((1 << collider.gameObject.layer) & collideLayer) != 0)
        //    {
        //        // Kill 파티클
        //        particles[i].remainingLifetime = 0; // 남은 생명 시간 0으로 설정
        //    }
        //}

        //// 업데이트된 파티클 배열을 다시 설정합니다.
        //ps.SetParticles(particles, count);
    }
}
