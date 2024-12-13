using UnityEngine;

/// <summary>
/// 총알을 나타내는 클래스입니다. 발사 후 일정 시간 동안 목표를 향해 비행하며, 
/// 충돌 후 폭발하여 주변의 대상에 피해를 입힙니다.
/// </summary>
public class Bullet : Projectile
{
    public enum ShotType
    {
        HIGHEST_SHOT, // 가장 높은 고도로 발사하여 목표에 도달합니다.
        LOWEST_SPEED, // 가장 낮은 속도로 목표에 도달하는 경로입니다.
        MOST_DIRECT // 가장 직선적인 경로로 목표에 도달합니다.
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
        // 8초가 지나면 자동으로 오브젝트가 제거되도록 하드 코딩
        // 생존 시간 변수를 따로 뒀지만 0으로 초기화 하는 바람에 부득이하게 하드 코딩
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
            // 발사되자마자 터지는 것을 막기 위해 0.5초 후에만 충돌을 활성화합니다.
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
    /// 목표 지점과 함께 총알을 발사합니다.
    /// </summary>
    /// <param name="target">목표 지점</param>
    /// <param name="shooter">발사한 무기</param>
    public override void Shot(Vector3 target, RangeWeapon shooter)
    {
        _rigidBody.isKinematic = false;

        _shooter = shooter;

        _rigidBody.velocity = GetVelocity(target);
		
		//작업중
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
    /// 목표 지점으로 이동하기 위한 속도를 계산합니다.
    /// </summary>
    /// <param name="target">목표 지점</param>
    /// <returns>목표 지점으로 향하는 속도 벡터</returns>
    private Vector3 GetVelocity(Vector3 target)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 toTarget = target - transform.position;

        // 이차 방정식을 푸는 데 필요한 조건을 설정합니다.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = projectileSpeed * projectileSpeed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // 최대 속도 이하로 목표에 도달할 수 있는지 확인합니다.
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
                    // 가장 높은 높이로 곡선을 그리며 명중합니다:
                    float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);
                    T = T_max;
                }
                break;
            case ShotType.LOWEST_SPEED:
                {
                    // 가장 낮은 곡선을 그리며 명중합니다.
                    float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
                    T = T_lowEnergy;
                }
                break;
            case ShotType.MOST_DIRECT:
                {
                    // 일직선으로 명중합니다.:
                    float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
                    T = T_min;
                }
                break;
            default:
                break;
        }

        // time-to-hit에서 launch velocity로 변환합니다:
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
