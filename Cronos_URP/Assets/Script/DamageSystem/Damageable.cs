using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Message;
using Sonity.Internal;
using Sonity;

public partial class Damageable : MonoBehaviour
{
    [Tooltip("피해를 받은 후 무적 상태가 되는 시간입니다.")]
    public float hitPoints;
    public float invulnerabiltyTime;


    [Tooltip("대미지를 입힐 수 있는 각도입니다. 항상 월드 XZ 평면에 있으며, 전방은 hitForwardRoation으로 회전합니다.")]
    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;
    [Tooltip("타격 각도 영역을 정의하는 기준 각도를 회전시킬 수 있습니다.")]
    [Range(0.0f, 360.0f)]
    [FormerlySerializedAs("hitForwardRoation")] //SHAME!
    public float hitForwardRotation = 360.0f;

    public Defensible defensible;

    public bool isInvulnerable { get; set; }
    private bool isVulnerable { get; set; } 

    public float currentHitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvulnerable, OnBecomeVulnerable, OnResetDamage;

    [Tooltip("데미지를 입으면, 다른 게임 오브젝트에게 메시지를 전달합니다.")]
    [EnforceType(typeof(Message.IMessageReceiver))]
    public List<MonoBehaviour> onDamageMessageReceivers;

    protected float m_timeSinceLastHit = 0.0f;
    protected Collider m_Collider;

    System.Action schedule;

    SoundManager soundManager;
    EffectManager effectManager;
    Player player;
    ImpulseCam impCam;
    [SerializeField] GameObject playerSword;

    void Start()
    {
        ResetDamage();
        m_Collider = GetComponent<Collider>();
        impCam = ImpulseCam.Instance;
    }

    private void OnEnable()
    {
        effectManager = EffectManager.Instance;
        soundManager = SoundManager.Instance;
        // Find의 비용이 크다고 하지만... 인스펙터에서 전부 넣기엔 귀찮다
        player = Player.Instance;
        playerSword = GameObject.Find("Sword");
    }

    void Update()
    {
        if (isInvulnerable)
        {
            m_timeSinceLastHit += Time.deltaTime;
            if (m_timeSinceLastHit > invulnerabiltyTime)
            {
                m_timeSinceLastHit = 0.0f;
                isInvulnerable = false;
                OnBecomeVulnerable.Invoke();
            }
        }
    }

    public void ResetDamage()
    {
        currentHitPoints = hitPoints;
        isInvulnerable = false;
        m_timeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetVulnerability(bool isVulnerable)
    {
        this.isVulnerable = isVulnerable;
    }

    public void SetColliderState(bool enabled)
    {
        m_Collider.enabled = enabled;
    }



    public void ApplyDamage(DamageMessage data)
    {
        if (currentHitPoints <= 0)
        {
            // 이미 죽은 상태라면 데미지를 더는 받지 않는다.
            // 만일 이미 죽은 뒤에도 데미지를 받는 것을 감지하고 싶다면 이부분 수정할 것
            //return;
        }

        if (isInvulnerable)
        {
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        // 데미지를 입은 방향과 데미지 영역을 투영(projection)
        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
            return;

        /// 여기에 민동휘가 만들어놓음
        /// 플레이어 소드를 LookAt 하여 파티클 인스턴싱
        /// 태그가 플레이어가 아니면 적이겠지
        if (gameObject.tag != "Player")
        {
            // 파편 만들기
            //Vector3 damagedPosition = transform.position;
            //// 일단 맞은 위치에 인스턴스를 만든다.
            //GameObject frag = effectManager.SpawnEffect("FragFX", damagedPosition);
            //frag.transform.LookAt(playerSword.transform);
            //frag.transform.Rotate(-15f, 0, 0);
            //Destroy(frag, 2.0f);

            // 피격 이펙트 만들기
            Vector3 dir = (transform.position - playerSword.transform.position).normalized;
            Vector3 newPos = new Vector3(transform.position.x - dir.x, playerSword.transform.position.y, transform.position.z - dir.z);
            GameObject slashed = effectManager.SpawnEffect("UpSlash", newPos);
            slashed.transform.forward = Camera.main.transform.forward;
            Destroy(slashed, 1.0f);

            // 히트 스탑
            if (player != null)
            {
                player.GetComponent<HitStop>().StartCoroutine(player.GetComponent<HitStop>().HitStopCoroutine());
            }
        }

        // 게임오브젝트 태그가 리스폰이면 == 적이면
        if (gameObject.tag == "Respawn")
        {
            impCam.Shake(impCam.shakeStrength);
        }

        if (defensible)
        {
            defensible.ApplyDamage(ref data);
        }

        isInvulnerable = true;
        currentHitPoints -= data.amount;

        // 죽든 살든 맞는 소리는 나와야 하니까
        if (gameObject.tag != "Player")
            soundManager.PlaySFX("Enemy_impact_SE", transform);

        if (currentHitPoints <= 0)
        {
            Debug.Log("데미지를 받아 죽었다");
            schedule += OnDeath.Invoke; //This avoid race condition when objects kill each other.
        }
        else
        {
            Debug.Log("데미지를 받았다");
            OnReceiveDamage.Invoke();
        }

        var messageType = currentHitPoints <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

        for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
        {
            var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public void JustDead()
    {
        Damageable.DamageMessage data;

        data.amount = 1;
        data.damager = this;
        data.direction = new Vector3(0, 0, 0);
        data.damageSource = new Vector3(0, 0, 0);
        data.throwing = false;
        data.stopCamera = false;

        for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
        {
            var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(MessageType.DEAD, this, data);
        }
    }

    void LateUpdate()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }

        UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 0.8f);
    }

    private void OnDrawGizmos()
    {
        if (isVulnerable)
        {
            var drawPos = transform.position;
            drawPos.y += 1.2f;

            Gizmos.color = new Color(1.0f, 0.9f, 0.0f, 0.5f);
            Gizmos.DrawSphere(drawPos, 0.5f);
        }
    }
#endif
}