using Message;
using UnityEngine;

/// <summary>
/// TestEnemy �� �ൿ�� �����Ѵ�.
/// </summary>
[DefaultExecutionOrder(100)]
public class TestEnemyBehavior : MonoBehaviour, IMessageReceiver
{
    // �ִϸ������� �Ķ���� 
    public static readonly int hashAttack = Animator.StringToHash("attack");
    public static readonly int hashInPursuit = Animator.StringToHash("inPursuit");
    public static readonly int hashNearBase = Animator.StringToHash("nearBase");
	public static readonly int hashDamageBase = Animator.StringToHash("damage");


	public SimpleDamager meleeWeapon;
    public FanShapeScanner playerScanner = new FanShapeScanner();
    public float timeToStopPursuit;

    [System.NonSerialized]
    public float attackDistance = 2;

    public GameObject Target { get { return _target; } }
    public Vector3 originalPosition { get; protected set; }
    public EnemyController controller { get { return _controller; } }
    public TargetDistributor.TargetFollower followerData { get { return _followerInstance; } }

    public GameObject _target;
    private EnemyController _controller;
    protected TargetDistributor.TargetFollower _followerInstance;
	protected Damageable _damageable;
    protected BulletTimeScalable _bulletTimeScalable;

	protected float _timerSinceLostTarget = 0.0f;

    void Awake()
    {
        _controller = GetComponentInChildren<EnemyController>();
		_damageable = GetComponent<Damageable>();

        if (meleeWeapon == null)
        {
            meleeWeapon = GetComponentInChildren<SimpleDamager>();
        }

    }

    void OnEnable()
    {
        SceneLinkedSMB<TestEnemyBehavior>.Initialise(_controller.animator, this);
		_damageable.onDamageMessageReceivers.Add(this);

        _bulletTimeScalable = GetComponent<BulletTimeScalable>();

		playerScanner.target = _target;

        originalPosition = transform.position;
    }

    protected void OnDisable()
    {
		_damageable.onDamageMessageReceivers.Remove(this);
		if (_followerInstance != null)
            _followerInstance.distributor.UnregisterFollower(_followerInstance);
    }

	void Damaged(Damageable.DamageMessage damageMessage)
	{
		_controller.animator.SetTrigger(hashDamageBase);
        SetBulletTimeScalable(false);
    }

    private void Update() 
    {
        LookAtTarget();
    }

	private void FixedUpdate()
    {
        Vector3 toBase = originalPosition - transform.position;
        toBase.y = 0;

        SetNearBase(toBase.sqrMagnitude < 1f); // 1 �� ���� ����
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

	public void FindTarget()
    {
        // Ÿ���� �̹� ���̴� ��� ���� ���̸� �����Ѵ�.
        var target = playerScanner.Detect(transform, _target == null);

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

                    if (toTarget.sqrMagnitude > playerScanner.detectionRadius * playerScanner.detectionRadius)
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
        if (_target == null) return;

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
        }

        SetInPursuit(true);

        _controller.SetRotationLerpSeedNormal();
    }

    public void StopPursuit()
    {
        if (_followerInstance != null)
        {
            _followerInstance.requireSlot = false;
        }

        SetInPursuit(false);
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

    public void SetNearBase(bool nearBase)
    {
        _controller.animator.SetBool(hashNearBase, nearBase);
    }

    private void SetInPursuit(bool inPursuit)
    {
        _controller.animator.SetBool(hashInPursuit, inPursuit);
    }

//     private void OnDrawGizmos()
//     {
//         if (playerScanner != null)
//         {
//             playerScanner.EditorGizmo(transform);
//         }
//     }

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

    public void SetVulnerable()
    {
        _damageable.SetVulnerability(true);
    }

    public void SetUnvulnerable()
    {
        _damageable.SetVulnerability(false);
    }

    public void SetBulletTimeScalable(bool val)
    {
        _bulletTimeScalable.active = val;
    }
}
