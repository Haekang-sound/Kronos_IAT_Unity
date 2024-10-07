using Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(EnemyController))]
public class BTypeEnemyBehavior : CombatZoneEnemy, IMessageReceiver
{
    public readonly float tp = 24;
    public float attackDistance = 1.8f;
    public float strafeSpeed = 1f;
    public float rotationSpeed = 1.0f;

    public GameObject aimRing;
    public GameObject aimEnd;
    private Vector3 ringOriginScale = new Vector3(1f, 1f, 1f);
    private Vector3 ringShrinkScale = new Vector3(0.5f, 0.5f, 0.5f);

    public Vector3 BasePosition { get; private set; }
    private float _baseTolerance = 0.6f;

    public EnemyController Controller { get { return _controller; } }

    private HitShake _hitShake;
    private KnockBack _knockBack;
    private Damageable _damageable;
    private RangeWeapon _rangeWeapon;
    private EnemyController _controller;
    private BulletTimeScalable _bulletTimeScalable;
    private SimpleDamager _meleeWeapon;
    private Rigidbody _rigidbody;

    // Animator Parameters
    public static readonly int hashAim = Animator.StringToHash("aim");
    public static readonly int hashDown = Animator.StringToHash("down");
    public static readonly int hashReturn = Animator.StringToHash("return");
    public static readonly int hashDamage = Animator.StringToHash("damage");
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");
    public static readonly int hashParsuit = Animator.StringToHash("pursuit");
    public static readonly int hashIdle = Animator.StringToHash("idle");
    public static readonly int hashParriableAttack = Animator.StringToHash("parriableAttack");

    void Awake()
    {
        BasePosition = transform.position;

        _hitShake = GetComponent<HitShake>();
        _knockBack = GetComponent<KnockBack>();
        _damageable = GetComponent<Damageable>();
        _rangeWeapon = GetComponent<RangeWeapon>();
        _controller = GetComponent<EnemyController>();
        _bulletTimeScalable = GetComponent<BulletTimeScalable>();
        _meleeWeapon = GetComponentInChildren<SimpleDamager>();
		_rigidbody = GetComponent<Rigidbody>();
    }

    // void Start()


    void OnEnable()
    {
        SceneLinkedSMB<BTypeEnemyBehavior>.Initialise(_controller.animator, this);

        _damageable.onDamageMessageReceivers.Add(this);

        _rigidbody.GetComponent<Rigidbody>();

        _rigidbody.drag = 10f;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        aimRing.SetActive(false);
        aimEnd.SetActive(false);
    }

    private void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
    }

    //void Update()

    // void FixedUpdate()

    // Debug ///////////////////////////////////////////////////////////////////////////////////

    public void ChangeDebugText(string state = nameof(BTypeEnemyBehavior))
    {
        var debugUI = GetComponentInChildren<TextMeshProUGUI>();

        if (debugUI != null)
        {
            debugUI.text = state;
        }

    }

    private void OnDrawGizmos()
    {
        if(drawGizmos == false) return;

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, strafeDistance);

        // 기본 위치 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(BasePosition, _baseTolerance);
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    public void BeginAttack()
    {
        if (CurrentTarget == null) return;

        var attackTarget = CurrentTarget.transform.position;

        // 공격 높이 오프셋
        attackTarget.y += 1.2f;

        _rangeWeapon.Attack(attackTarget);
    }

    public void EndAttack()
    {

    }

    public bool IsNearBase()
    {
        Vector3 toBase = BasePosition - transform.position;
        return toBase.sqrMagnitude < _baseTolerance;
    }

    public bool IsInAttackRange()
    {
        return CheckDistanceWithTarget(attackDistance);
    }

    public bool IsLookAtTarget()
    {
        float angleThreshold = 10.0f; // 바라보는 것으로 간주할 최대 각도

        if (CurrentTarget == null) return false;

        Vector3 forward = transform.forward;
        Vector3 toTarget = (CurrentTarget.transform.position - transform.position).normalized;

        // 두 벡터 간의 각도 계산
        float angle = Vector3.Angle(forward, toTarget);

        // 각도가 임계값 이하이면 true 반환
        return angle <= angleThreshold;
    }

    private bool CheckDistanceWithTarget(float distance)
    {
        Vector3 toTarget = CurrentTarget.transform.position - transform.position;
        return toTarget.sqrMagnitude < distance * distance;
    }


    public void StrafeLeft()
    {
        if (CurrentTarget == null) return;

        // 이동 목적지 설정
        var offsetPlayer = transform.position - CurrentTarget.transform.position;
        var direction = Vector3.Cross(offsetPlayer, Vector3.up);
        _controller.SetTarget(transform.position + direction);

        LookAtTarget();
    }

    public void StrafeRight() 
    {
        if (CurrentTarget == null) return;

        // 이동 목적지 설정
        var offsetPlayer = CurrentTarget.transform.position - transform.position;
        var direction = Vector3.Cross(offsetPlayer, Vector3.up);
        _controller.SetTarget(transform.position + direction);

        LookAtTarget();
    }

    public void LookAtTarget()
    {
        if (CurrentTarget == null) return;

        // 바라보는 방향 설정
        var lookPosition = CurrentTarget.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        var dmgMsg = (Damageable.DamageMessage)data;

        switch (type)
        {
            case MessageType.DAMAGED:
                EffectManager.Instance.CreateHitFX(dmgMsg, transform);
                Damaged(dmgMsg);
                break;
            case MessageType.DEAD:
                EffectManager.Instance.CreateHitFX(dmgMsg, transform);
                Dead();
                break;
            case MessageType.RESPAWN:
                break;
            default:
                return;

        }
    }

    private bool useKnockback;
    public void SetUseKnockback(bool val) => useKnockback = val;


    private void Damaged(Damageable.DamageMessage msg)
    {
		Player.Instance.ChargeCP(msg.isActiveSkill);
		UnuseBulletTimeScale();
        TriggerDamage();
        _hitShake.Begin();

		if (Player.Instance != null)
		{
			Player.Instance.TP += Player.Instance.TPGain();
		}

		if (useKnockback)
        {
            _knockBack?.Begin(msg.damageSource);
        }
    }

    private void Dead()
    {
        if (Player.Instance != null)
        {
            Player.Instance.TP += tp;
        }

        GetComponent<ReplaceWithRagdoll>().Replace();
        _controller.Release();
    }

    internal void UseBulletTimeScale()
    {
        _bulletTimeScalable.active = true;
    }

    internal void UnuseBulletTimeScale()
    {
        _bulletTimeScalable.active = false;
    }

    internal void SetFollowerDataRequire(bool val)
    {
        FollowerData.requireSlot = val;
    }

    internal void TriggerAim()
    {
        _controller.animator.SetTrigger(hashAim);
    }

    internal void TriggerDown()
    {
        _controller.animator.SetTrigger(hashDown);
    }

    internal void TriggerReturn()
    {
        _controller.animator.SetTrigger(hashReturn);
    }

    internal void TriggerDamage()
    {
        _controller.animator.SetTrigger(hashDamage);
    }
    internal void ResetTriggerDamaged()
    {
        _controller.animator.ResetTrigger(hashDamage);
    }

    internal void TriggerAttack()
    {
        _controller.animator.SetTrigger(hashAttack);
    }

    internal void TriggerParriableAttack()
    {
        _controller.animator.SetTrigger(hashParriableAttack);
    }

    internal void TriggerPursuit()
    {
        _controller.animator.SetTrigger(hashParsuit);
    }

    internal void RequestTargetPosition()
    {
        RequestTargetPosition(attackDistance);
    }

    internal void TriggerIdle()
    {
        _controller.animator.SetTrigger(hashIdle);
    }

    // 에임 이펙트 관련한거
    public IEnumerator ShrinkScale()
    {
        aimRing.transform.localScale = ringOriginScale;
        float elapsedTime = 0.0f;
        while (elapsedTime < 2.0f)
        {
            aimRing.transform.localScale = Vector3.Lerp(ringOriginScale, ringShrinkScale, elapsedTime / 2.0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
