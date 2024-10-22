using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCombatZoneEnemyBehavior : MonoBehaviour, IMessageReceiver
{
    // �ִϸ������� �Ķ���� 
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");
    public static readonly int hashDamageBase = Animator.StringToHash("damage");

    public MeleeWeapon meleeWeapon;

    public float timeToStopPursuit;

    //[System.NonSerialized]
    public float attackDistance = 2;

    public CombatZone combatZone { private get; set; }
    public GameObject target { get { return _target; } }
    public Vector3 originalPosition { get; protected set; }
    public EnemyController controller { get { return _controller; } }
    public TargetDistributor.TargetFollower followerData { get { return _followerInstance; } }

    private GameObject _target;
    private EnemyController _controller;
    protected TargetDistributor.TargetFollower _followerInstance;
    protected Damageable _damageable;
    private BulletTimeScalable _bulletTimeScalable;

    protected float _timerSinceLostTarget = 0.0f;

    void Awake()
    {
        _controller = GetComponentInChildren<EnemyController>();

        if (meleeWeapon == null)
        {
            meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        }
    }

    void OnEnable()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.onDamageMessageReceivers.Add(this);

        _bulletTimeScalable = GetComponent<BulletTimeScalable>();

        SceneLinkedSMB<TestCombatZoneEnemyBehavior>.Initialise(_controller.animator, this);

        originalPosition = transform.position;
    }

    protected void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);

        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
    }
    private void FixedUpdate()
    {
        LookAtTarget();

        Vector3 toBase = originalPosition - transform.position;
        toBase.y = 0;

        SetNearBase(toBase.sqrMagnitude < 1f);
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Damaged(damageData);
                }
                break;
            case MessageType.DEAD:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Death(damageData);
                }
                break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage)
    {
        _controller.animator.SetTrigger(hashDamageBase);
        SetBulletTimeScalable(false);
    }

    public void FindTarget()
    {
        // Ÿ���� �̹� ���̴� ��� ���� ���̸� �����Ѵ�.
        var target = combatZone.Detect(transform, _target == null);

        if (_target == null)
        {
            // �÷��̾ ó�� �� ���, �ֺ��� �� ������ �����Ͽ� Ÿ����.
            // (���� �÷��̾� ��ġ�� �̵����� �ʰ� �÷��̾� �ֺ��� ������ �̷絵��)
            if (target != null)
            {
                _target = target;
                TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                if (distributor != null)
                {
                    _followerInstance = distributor.RegisterNewFollower();
                }
            }
        }
        else
        {
            // �÷��̾ ���� ������ ����� ���� �ð� ���� �÷��̾ ������ �ʾƵ�
            // ���� ������ ��� �� �ƴϸ� ��� ���� �Ѵ�.
            if (target == null)
            {
                _timerSinceLostTarget += Time.deltaTime;

                if (_timerSinceLostTarget >= timeToStopPursuit)
                {
                    Vector3 toTarget = _target.transform.position - transform.position;

                    //if (toTarget.sqrMagnitude > playerScanner.detectionRadius * playerScanner.detectionRadius)
                    {
                        if (_followerInstance != null)
                            _followerInstance.distributor.UnregisterFollower(_followerInstance);

                        // Ÿ���� Ž�� ������ ����� Ÿ���� �缳���Ѵ�.
                        _target = null;
                    }
                }
            }
            else
            {
                if (target != _target)
                {
                    if (_followerInstance != null)
                    {
                        _followerInstance.distributor.UnregisterFollower(_followerInstance);
                    }

                    _target = target;

                    TargetDistributor distributor = target.GetComponentInChildren<TargetDistributor>();
                    if (distributor != null)
                    {
                        _followerInstance = distributor.RegisterNewFollower();
                    }
                }

                _timerSinceLostTarget = 0.0f;
            }
        }
    }

    public void LookAtTarget()
    {
        if (_target == null)
        {
            return;
        }

        _controller.SetForwardToTarget(_target.transform.position);

    }

    public void StartLookAtTarget()
    {
        _controller.SetRotationLerpSeedSlow();
    }

    public void StopLookAtTarget()
    {
        _controller.SetRotationLerpSeedZero();
    }

    public void StartPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = true;
            RequestTargetPosition();
            SetInPursuit(true);
        }


        _controller.SetRotationLerpSeedNormal();
    }

    public void StopPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = false;
            SetInPursuit(false);
        }

    }

    public void RequestTargetPosition()
    {
        Vector3 fromTarget = transform.position - _target.transform.position;
        fromTarget.y = 0;

        _followerInstance.requiredPoint = _target.transform.position + fromTarget.normalized * attackDistance * 0.9f;
    }

    public void WalkBackToBase()
    {
        if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
        _target = null;
        StopPursuit();
        _controller.SetTarget(originalPosition);
        _controller.SetFollowNavmeshAgent(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print(true);
            _target = collision.gameObject;
        }
    }

    public void TriggerAttack()
    {
        _controller.animator.SetTrigger(hashAttack);
    }
    public void AttackBegin()
    {
        meleeWeapon.BeginAttack();
    }

    public void AttackEnd()
    {
        meleeWeapon.EndAttack();
    }

    public void SetNearBase(bool nearBase)
    {
        _controller.animator.SetBool(hashNearBase, nearBase);
    }

    public void SetInPursuit(bool inPursuit)
    {
        _controller.animator.SetBool(hashInPursuit, inPursuit);
    }

    private void OnDrawGizmosSelected()
    {
        // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }

    public void Death(Damageable.DamageMessage msg)
    {
        var replacer = GetComponent<ReplaceWithRagdoll>();

        if (replacer != null)
        {
            replacer.Replace();
        }

        //We unparent the hit source, as it would destroy it with the gameobject when it get replaced by the ragdol otherwise
    }

    public void SetBulletTimeScalable(bool val)
    {
        _bulletTimeScalable.SetActive(val);
    }
}
