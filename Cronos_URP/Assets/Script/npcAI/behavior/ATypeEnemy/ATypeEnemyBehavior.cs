using Message;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(EnemyController))]
public class ATypeEnemyBehavior : CombatZoneEnemy, IMessageReceiver
{
    public float attackDistance = 1.8f;
    public float strongAttackDistance = 3f;
    public float strafeDistance = 2f;
    public float strafeSpeed = 1f;
    public float rotationSpeed = 1.0f;
    public Vector3 BasePosition { get; private set; }
    public EnemyController Controller { get { return _controller; } }

    private Damageable _damageable;
    private EnemyController _controller;
    private MeleeTriggerEnterDamager _meleeWeapon;

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

        _damageable = GetComponent<Damageable>();
        _controller = GetComponent<EnemyController>();
        _meleeWeapon = GetComponentInChildren<MeleeTriggerEnterDamager>();
    }

    // void Start()

    void OnEnable()
    {
        SceneLinkedSMB<ATypeEnemyBehavior>.Initialise(_controller.animator, this);

        _damageable.onDamageMessageReceivers.Add(this);
    }

    private void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
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
        if(drawGizmos == false) return;

        // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, strafeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, strongAttackDistance);
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    public bool IsNearBase()
    {
        Vector3 toBase = BasePosition - transform.position;
        return toBase.sqrMagnitude < 0.01f;  // 0.01 �� ���� ����
    }

    public void StrafeLeft()
    {
        if (CurrentTarget == null) return;

        // �̵� ������ ����
        var offsetPlayer = transform.position - CurrentTarget.transform.position;
        var direction = Vector3.Cross(offsetPlayer, Vector3.up);
        _controller.SetTarget(transform.position + direction);

        LookAtTarget();
    }

    public void StrafeRight() 
    {
        if (CurrentTarget == null) return;

        // �̵� ������ ����
        var offsetPlayer = CurrentTarget.transform.position - transform.position;
        var direction = Vector3.Cross(offsetPlayer, Vector3.up);
        _controller.SetTarget(transform.position + direction);

        LookAtTarget();
    }

    public void LookAtTarget()
    {
        if (CurrentTarget == null) return;

        // �ٶ󺸴� ���� ����
        var lookPosition = CurrentTarget.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                Damaged();
                break;
            case MessageType.DEAD:
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
        TriggerDamage();
    }

    private void Dead()
    {
        GetComponent<ReplaceWithRagdoll>().Replace();
    }

    public void BeginAttack()
    {
        _meleeWeapon.BeginAttack();
    }

    public void EndAttack()
    {
        _meleeWeapon.EndAttack();
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

    internal void TriggerDown()
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
