using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ���ֿ��� Bullet ��ũ��Ʈ�� �������ؼ� ������ ����ü�� ���� ��ũ��Ʈ
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
        // 8�ʰ� ������ �ڵ����� ������Ʈ�� ���ŵǵ��� �ϵ� �ڵ�
        // ���� �ð� ������ ���� ������ 0���� �ʱ�ȭ �ϴ� �ٶ��� �ε����ϰ� �ϵ� �ڵ�
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
            // �߻���ڸ��� ������ ���� ���� ���� 0.5�� �Ŀ��� �浹�� Ȱ��ȭ�մϴ�.
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

        // ������ �������̴ϱ� switch ���� ����
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
        T = T_min;

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
