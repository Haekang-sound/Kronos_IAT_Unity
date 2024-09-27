using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SimpleDamager : MonoBehaviour
{
    public bool drawGizmos;

    public float damageAmount = 1;
    public bool stopCamera;

    public LayerMask targetLayers;

    protected GameObject m_owner;
    public bool _inAttack;

    public delegate void TriggerEnterAction(Collider other);
    public event TriggerEnterAction OnTriggerEnterEvent;

    SoundManager soundManager;

    public UnityEvent OnAttack;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnEnable()
    {
        soundManager = SoundManager.Instance;
    }

    public void SetOwner(GameObject owner) => m_owner = owner;

    public void BeginAttack()
    {
        _inAttack = true;
    }

    public void EndAttack()
    {
        _inAttack = false;
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        DrawGizmos();
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageCheck(other);
    }

    public bool DamageCheck(Collider other)
    {
        var damageable = other.GetComponent<Damageable>();

        if(!_inAttack)
        {
            return false;
        }

        if (damageable == null)
        {
            return false;
        }

        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return false;
        }

        var msg = new Damageable.DamageMessage()
        {
            amount = damageAmount,

            damager = this,
            direction = (transform.position - other.transform.position).normalized,
            damageSource = transform.position,
            stopCamera = stopCamera
        };

        if (m_owner != null)
        {
            msg.damageSource = m_owner.transform.position;
        }

        damageable.ApplyDamage(msg);
        OnAttack.Invoke();

        return true;
    }

    public void DrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        var collider = GetComponent<Collider>();

        if (collider is BoxCollider boxCollider)
        {
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
        else if (collider is SphereCollider sphereCollider)
        {
            Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
        }
        else if (collider is CapsuleCollider capsuleCollider)
        {
            // 캡슐 콜라이더의 위치와 크기를 가져옵니다.
            Vector3 center = capsuleCollider.center;
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius;

            // 캡슐 콜라이더의 방향에 따라 캡슐의 상하 위치를 결정합니다.
            Vector3 upDirection;
            if (capsuleCollider.direction == 0) // X-Axis
            {
                upDirection = Vector3.right;
            }
            else if (capsuleCollider.direction == 1) // Y-Axis
            {
                upDirection = Vector3.up;
            }
            else // Z-Axis
            {
                upDirection = Vector3.forward;
            }

            Vector3 bottomSphereCenter = center - upDirection * (height - radius);
            Vector3 topSphereCenter = center + upDirection * (height - radius);

            // 반구 및 실린더 부분을 그립니다.
            Gizmos.DrawSphere(bottomSphereCenter, radius);
            Gizmos.DrawSphere(topSphereCenter, radius);
            Gizmos.DrawLine(bottomSphereCenter + radius * Vector3.forward, topSphereCenter + radius * Vector3.forward);
            Gizmos.DrawLine(bottomSphereCenter - radius * Vector3.forward, topSphereCenter - radius * Vector3.forward);
            Gizmos.DrawLine(bottomSphereCenter + radius * Vector3.right, topSphereCenter + radius * Vector3.right);
            Gizmos.DrawLine(bottomSphereCenter - radius * Vector3.right, topSphereCenter - radius * Vector3.right);
        }
    }
}