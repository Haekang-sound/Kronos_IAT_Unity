using Message;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(EnemyController))]
public class ATypeEnemyBehavior : CombatZoneEnemy, IMessageReceiver
{
    public readonly float tp = 20;
    public float attackDistance = 1.8f;
    public float strongAttackDistance = 3f;
    public float strafeDistance = 2f;
    public float strafeSpeed = 1f;
    public float rotationSpeed = 1.0f;

    public Vector3 BasePosition { get; private set; }
    private float _baseTolerance = 0.6f;

    public EnemyController Controller { get { return _controller; } }

    private HitShake _hitShake;
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
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashIdle = Animator.StringToHash("idle");
    public static readonly int hashParriableAttack = Animator.StringToHash("parriableAttack");

    void Awake()
    {
        BasePosition = transform.position;

        _hitShake = GetComponent<HitShake>();
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
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, strafeDistance);

        Gizmos.color = Color.yellow;
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
                Damaged();
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

    public bool IsInAttackRange()
    {
        return CheckDistanceWithTarget(attackDistance);
    }

    public bool CheckDistanceWithTarget(float distance)
    {
        Vector3 toTarget = CurrentTarget.transform.position - transform.position;
        return toTarget.sqrMagnitude < distance * distance;
    }

    private void Damaged()
    {
        UnuseBulletTimeScale();
        TriggerDamage();
        _hitShake.Begin();
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
        _bulletTimeScalable.active = true;
    }

    internal void UnuseBulletTimeScale()
    {
        _bulletTimeScalable.active = false;
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
        RequestTargetPosition(attackDistance);
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
