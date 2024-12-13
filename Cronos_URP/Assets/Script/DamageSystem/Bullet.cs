using UnityEngine;

/// <summary>
/// �Ѿ��� ��Ÿ���� Ŭ�����Դϴ�. �߻� �� ���� �ð� ���� ��ǥ�� ���� �����ϸ�, 
/// �浹 �� �����Ͽ� �ֺ��� ��� ���ظ� �����ϴ�.
/// </summary>
public class Bullet : Projectile
{
    public enum ShotType
    {
        HIGHEST_SHOT, // ���� ���� ���� �߻��Ͽ� ��ǥ�� �����մϴ�.
        LOWEST_SPEED, // ���� ���� �ӵ��� ��ǥ�� �����ϴ� ����Դϴ�.
        MOST_DIRECT // ���� �������� ��η� ��ǥ�� �����մϴ�.
    }

    public ShotType shotType;
    public float projectileSpeed;
    public int damageAmount = 1;
    public LayerMask damageMask;
	public Damageable.DamageType damageType;
    public float explosionRadius;
    public float explosionTimer;


	protected float _sinceFired;

    protected RangeWeapon _shooter;
    protected Rigidbody _rigidBody;

    protected static Collider[] _explosionHitCache = new Collider[32];

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.detectCollisions = false;
    }

    private void OnEnable()
    {
        _rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _rigidBody.isKinematic = true;
        _sinceFired = 0.0f;
    }

    private void Update()
    {
        // 8�ʰ� ������ �ڵ����� ������Ʈ�� ���ŵǵ��� �ϵ� �ڵ�
        // ���� �ð� ������ ���� ������ 0���� �ʱ�ȭ �ϴ� �ٶ��� �ε����ϰ� �ϵ� �ڵ�
        if (_sinceFired > 8f)
        {
            pool.Free(this);
        }
    }

	private Vector3 _currentVelocity;
    private void FixedUpdate()
    {
		_rigidBody.velocity = _currentVelocity * BulletTime.Instance.GetCurrentSpeed();

		_sinceFired += Time.deltaTime;

        if (_sinceFired > 0.2f)
        {
            // �߻���ڸ��� ������ ���� ���� ���� 0.5�� �Ŀ��� �浹�� Ȱ��ȭ�մϴ�.
            _rigidBody.detectCollisions = true;
        }

        if (explosionTimer > 0 && _sinceFired > explosionTimer)
        {
            Explosion();
        }
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (explosionTimer < 0)
        {
            Explosion();
        }
    }

    /// <summary>
    /// ��ǥ ������ �Բ� �Ѿ��� �߻��մϴ�.
    /// </summary>
    /// <param name="target">��ǥ ����</param>
    /// <param name="shooter">�߻��� ����</param>
    public override void Shot(Vector3 target, RangeWeapon shooter)
    {
        _rigidBody.isKinematic = false;

        _shooter = shooter;

        _rigidBody.velocity = GetVelocity(target);
		
		//�۾���
		_currentVelocity = _rigidBody.velocity;

		_rigidBody.AddRelativeTorque(Vector3.right * -5500.0f);

        _rigidBody.detectCollisions = false;

        transform.forward = target - transform.position;
    }

    public void Explosion()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _explosionHitCache,
                damageMask.value);

        Damageable.DamageMessage message = new Damageable.DamageMessage
        {
            amount = damageAmount,
            damageSource = transform.position,
			damageType = damageType,
            damager = this,
            throwing = true
        };


        for (int i = 0; i < count; ++i)
        {
            Damageable d = _explosionHitCache[i].GetComponentInChildren<Damageable>();

            if (d != null)
                d.ApplyDamage(message);
        }

        pool.Free(this);
    }

    /// <summary>
    /// ��ǥ �������� �̵��ϱ� ���� �ӵ��� ����մϴ�.
    /// </summary>
    /// <param name="target">��ǥ ����</param>
    /// <returns>��ǥ �������� ���ϴ� �ӵ� ����</returns>
    private Vector3 GetVelocity(Vector3 target)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 toTarget = target - transform.position;

        // ���� �������� Ǫ�� �� �ʿ��� ������ �����մϴ�.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = projectileSpeed * projectileSpeed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // �ִ� �ӵ� ���Ϸ� ��ǥ�� ������ �� �ִ��� Ȯ���մϴ�.
        if (discriminant < 0)
        {
            velocity = toTarget;
            velocity.y = 0;
            velocity.Normalize();
            velocity.y = 0.7f;

            Debug.DrawRay(transform.position, velocity * 3.0f, Color.blue);

            velocity *= projectileSpeed;
            return velocity;
        }

        float discRoot = Mathf.Sqrt(discriminant);

        float T = 0;
        switch (shotType)
        {
            case ShotType.HIGHEST_SHOT:
                {
                    // ���� ���� ���̷� ��� �׸��� �����մϴ�:
                    float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);
                    T = T_max;
                }
                break;
            case ShotType.LOWEST_SPEED:
                {
                    // ���� ���� ��� �׸��� �����մϴ�.
                    float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
                    T = T_lowEnergy;
                }
                break;
            case ShotType.MOST_DIRECT:
                {
                    // ���������� �����մϴ�.:
                    float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
                    T = T_min;
                }
                break;
            default:
                break;
        }

        // time-to-hit���� launch velocity�� ��ȯ�մϴ�:
        velocity = toTarget / T - Physics.gravity * T / 2f;

        return velocity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
