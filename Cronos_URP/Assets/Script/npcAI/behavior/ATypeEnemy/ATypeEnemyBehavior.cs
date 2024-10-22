using Message;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(FanShapeScanner))]
public class ATypeEnemyBehavior : FanShapeScannerEnemy, IMessageReceiver
{
    [SerializeField]
    private bool drawGizmos = true;

    public readonly float tp = 20;

    [Header("Attack Ragne")]
    public float meleeAttackDistance = 1.8f;
    public float normalAttackDistance = 2.4f;
    public float strongAttackDistance = 3f;

    [Header("Speed")]
    public float strafeSpeed = 1f;
    public float rotationSpeed = 1.0f;

    public Vector3 BasePosition { get; private set; }
    private float _baseTolerance = 0.6f;

    [HideInInspector]
    public bool inAttack;

    public EnemyController Controller { get { return _controller; } }

    private HitShake _hitShake;
    private KnockBack _knockBack;
    private Damageable _damageable;
    private EnemyController _controller;
    private BulletTimeScalable _bulletTimeScalable;
    private MeleeWeapon _meleeWeapon;
    private Rigidbody _rigidbody;

    // Animator Parameters
    public static readonly int hashDown = Animator.StringToHash("down");
    public static readonly int hashReturn = Animator.StringToHash("return");
    public static readonly int hashStrafe = Animator.StringToHash("strafe");
    public static readonly int hashDamage = Animator.StringToHash("damage");
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashAttackNormal = Animator.StringToHash("attack_normal");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashIdle = Animator.StringToHash("idle");
    public static readonly int hashParriableAttack = Animator.StringToHash("parriableAttack");

    void Awake()
    {
        BasePosition = transform.position;

        _hitShake = GetComponent<HitShake>();
        _knockBack = GetComponent<KnockBack>();
        _damageable = GetComponent<Damageable>();
        _controller = GetComponent<EnemyController>();
        _bulletTimeScalable = GetComponent<BulletTimeScalable>();
        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    void Start()
    {
        OnDown.AddListener(TriggerDown);
    }


    void OnEnable()
    {
        SceneLinkedSMB<ATypeEnemyBehavior>.Initialise(_controller.animator, this);

        _damageable.onDamageMessageReceivers.Add(this);

        _meleeWeapon.SetOwner(gameObject);

        _rigidbody.GetComponent<Rigidbody>();

        _rigidbody.drag = 10f;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
    }

    void OnDestroy()
    {
        OnDown?.RemoveListener(TriggerDown);
    }

    // void Update()
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
        if (drawGizmos == false) return;

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, normalAttackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, strongAttackDistance);

        // 기본 위치 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(BasePosition, _baseTolerance);
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    public bool IsNearBase()
    {
        Vector3 toBase = BasePosition - transform.position;
        return toBase.sqrMagnitude < _baseTolerance;
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

    public bool IsInStrongAttackRange()
    {
        return CheckDistanceWithTarget(strongAttackDistance);
    }

    public bool IsInNormalAttackRange()
    {
        return CheckDistanceWithTarget(normalAttackDistance);
    }

    public bool IsInAttackRange()
    {
        return CheckDistanceWithTarget(meleeAttackDistance);
    }

    public bool CheckDistanceWithTarget(float distance)
    {
        Vector3 toTarget = CurrentTarget.transform.position - transform.position;
        return toTarget.sqrMagnitude < distance * distance;
    }

    private void Damaged(Damageable.DamageMessage msg)
    {
        if (inAttack && msg.comboType == Damageable.ComboType.UninterruptibleCombo) 
            return;

        Player.Instance.ChargeCP(msg.isActiveSkill);
        UnuseBulletTimeScale();
        TriggerDamage(msg.damageType);
        _hitShake.Begin();

        if (Player.Instance != null)
        {
            //Player.Instance.TP += Player.Instance.TPGain();
        }

        _knockBack?.Begin(msg.damageSource);
    }

    private void Dead()
    {
        GetComponent<ReplaceWithRagdoll>().Replace();
        _controller.Release();
    }

    public void BeginAttack()
    {
        _meleeWeapon.BeginAttack();
    }

    public void EndAttack()
    {
        _meleeWeapon.EndAttack();
    }

    public void BeginCanBeParried()
    {
        _meleeWeapon.BeginCanBeParried();
    }

    public void EndBeCanParried()
    {
        _meleeWeapon.EndBeCanParried();
    }

    public void BeginAiming()
    {
        rotationSpeed = 5f;
    }

    public void StopAiming()
    {
        rotationSpeed = 0f;
    }

    public void ResetAiming()
    {
        rotationSpeed = 1f;
    }

    private void SetInPursuit(bool inPursuit)
    {
        _controller.animator.SetBool(hashInPursuit, inPursuit);
    }

    public void StartPursuit()
    {
        if (FollowerData != null)
        {
            FollowerData.requireSlot = true;
            RequestTargetPosition();
        }

        SetInPursuit(true);
    }

    internal void UseBulletTimeScale()
    {
        _bulletTimeScalable.SetActive(true);
    }

    internal void UnuseBulletTimeScale()
    {
        _bulletTimeScalable.SetActive(false);
    }

    public void TriggerDown()
    {
        _controller.animator.SetTrigger(hashDown);
    }

    internal void ResetTriggerDown()
    {
        _controller.animator.ResetTrigger(hashDown);
    }

    internal void TriggerReturn()
    {
        _controller.animator.SetTrigger(hashReturn);
    }

    internal void TriggerDamage(Damageable.DamageType type)
    {
        var key = hashDamage;

        switch (type)
        {
            case Damageable.DamageType.None:
                break;
            case Damageable.DamageType.ATypeHit:
                key = Animator.StringToHash("hit_a");
                break;
            case Damageable.DamageType.BTypeHit:
                key = Animator.StringToHash("hit_b");
                break;
            case Damageable.DamageType.KockBack:
                key = Animator.StringToHash("knock_back");
                break;
            case Damageable.DamageType.Fall:
                key = Animator.StringToHash("fall");
                break;
            case Damageable.DamageType.OnFallDamaged:
                key = Animator.StringToHash("on_fall_damaged");
                break;
            case Damageable.DamageType.Down:
                key = Animator.StringToHash("fall_down");
                break;
        }

        _controller.animator.SetTrigger(key);
    }

    internal void ResetTriggerDamaged()
    {
        _controller.animator.ResetTrigger(hashDamage);
    }

    internal void TriggerAttack()
    {
        _controller.animator.SetTrigger(hashAttack);
    }

    internal void TriggerAttackNormal()
    {
        _controller.animator.SetTrigger(hashAttackNormal);
    }

    internal void TriggerStrongAttack()
    {
        _controller.animator.SetTrigger(hashParriableAttack);
    }

    internal void StopPursuit()
    {
        if (FollowerData != null)
        {
            FollowerData.requireSlot = false;
        }

        SetInPursuit(false);
    }

    internal void RequestTargetPosition()
    {
        RequestTargetPosition(meleeAttackDistance);
    }

    internal void TriggerStrafe()
    {
        _controller.animator.SetTrigger(hashStrafe);
    }

    internal void TriggerIdle()
    {
        _controller.animator.SetTrigger(hashIdle);
    }
}
