using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 김주영의 Bullet 스크립트를 마개조해서 보스용 투사체로 만든 스크립트
public class BossBullet : Projectile
{
    public enum ShotType
    {
        MOST_DIRECT
    }

    public ShotType shotType;
    public float projectileSpeed;
    public int damageAmount = 1;
    public LayerMask damageMask;
    public float explosionRadius;
    public float explosionTimer;

    protected float m_sinceFired;

    protected RangeWeapon m_shooter;
    protected Rigidbody m_rigidBody;

    protected static Collider[] m_ExplosionHitCache = new Collider[32];

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.detectCollisions = false;
    }

    private void OnEnable()
    {
        m_rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        m_rigidBody.isKinematic = true;
        m_sinceFired = 0.0f;
    }

    private void Update()
    {
        // 8초가 지나면 자동으로 오브젝트가 제거되도록 하드 코딩
        // 생존 시간 변수를 따로 뒀지만 0으로 초기화 하는 바람에 부득이하게 하드 코딩
        if (m_sinceFired > 8f)
        {
            pool.Free(this);
        }
    }

    private void FixedUpdate()
    {
        m_sinceFired += Time.deltaTime;

        if (m_sinceFired > 0.2f)
        {
            // 발사되자마자 터지는 것을 막기 위해 0.5초 후에만 충돌을 활성화합니다.
            m_rigidBody.detectCollisions = true;
        }

        if (explosionTimer > 0 && m_sinceFired > explosionTimer)
        {
            Explosion();
        }
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (other.GetType() ==  typeof(Player))
        {

        }

        if (explosionTimer < 0)
        {
            Explosion();
        }
    }

    public override void Shot(Vector3 target, RangeWeapon shooter)
    {
        m_rigidBody.isKinematic = false;

        m_shooter = shooter;

        m_rigidBody.velocity = GetVelocity(target);
        m_rigidBody.AddRelativeTorque(Vector3.right * -5500.0f);

        m_rigidBody.detectCollisions = false;

        transform.forward = target - transform.position;
    }

    public void Explosion()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, m_ExplosionHitCache,
                damageMask.value);

        Damageable.DamageMessage message = new Damageable.DamageMessage
        {
            amount = damageAmount,
            damageSource = transform.position,
            damager = this,
            throwing = true
        };


        for (int i = 0; i < count; ++i)
        {
            Damageable d = m_ExplosionHitCache[i].GetComponentInChildren<Damageable>();

            if (d != null)
                d.ApplyDamage(message);
        }

        pool.Free(this);
    }

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

        // 무조건 일직선이니까 switch 문은 삭제
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
        T = T_min;

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
