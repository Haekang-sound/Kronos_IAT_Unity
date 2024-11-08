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

    // ����Ʈ�Ǵ� ��ƼŬ ������ ���ϴ� ������Ƽ
    public float BurstNumber
    {
        get { return burstNumber; }
        set { burstNumber = value; }
    }

    // ���� ���� ��ƼŬ �ý����� ����Ʈ ī��Ʈ�� �����
    public void RegisterBurst(float bNum)
    {
        var emitModule = ps.emission;

        ParticleSystem.Burst burst = emitModule.GetBurst(0);
        burst.count = new ParticleSystem.MinMaxCurve(bNum);

        emitModule.SetBurst(0, burst);
        Debug.Log("Burst number : " + bNum);
    }

    // 1�� ��ٸ� �� ��ƼŬ �ý����� �ܺ� ���� on��
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

        // ��ƼŬ Ʈ���� �Ǿ��� �� TP HUD �ʷϻ�����
        UI_TPCPHUD uiHud = UI_TPCPHUD.Instance;
        uiHud.ChangeGreen();

        // ��ƼŬ �迭 ���� �� ��������
        //ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        //int count = ps.GetParticles(particles);

        //for (int i = 0; i < count; i++)
        //{
        //    // Ư�� ���ǿ� ���� Kill�� �����մϴ�.
        //    // ���� ���, Ʈ���ŵ� ��ƼŬ�� ���̾ targetLayer�� ��ġ�� �� Kill�մϴ�.
        //    Collider collider = Physics.OverlapSphere(particles[i].position, 0.1f)[0];
        //    if (collider != null && ((1 << collider.gameObject.layer) & collideLayer) != 0)
        //    {
        //        // Kill ��ƼŬ
        //        particles[i].remainingLifetime = 0; // ���� ���� �ð� 0���� ����
        //    }
        //}

        //// ������Ʈ�� ��ƼŬ �迭�� �ٽ� �����մϴ�.
        //ps.SetParticles(particles, count);
    }
}
