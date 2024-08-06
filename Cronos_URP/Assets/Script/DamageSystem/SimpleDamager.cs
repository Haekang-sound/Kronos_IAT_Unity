using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SimpleDamager : MonoBehaviour
{
    public bool drawGizmos;

    public float damageAmount = 1;
    public bool stopCamera = false;

    public LayerMask targetLayers;

    //protected GameObject m_owner;
    protected bool m_inAttack = false;

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

    //public void SetOwner(GameObject owner)
    //{
    //    m_owner = owner;
    //}

    public void BeginAttack()
    {
        m_inAttack = true;
    }

    public void EndAttack()
    {
        m_inAttack = false;
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

    public void DamageCheck(Collider other)
    {
        if (!m_inAttack)
        {
            return;
        }

        if (this.CompareTag("Player"))
        {
            OnTriggerEnterEvent(other);
        }

        var damageable = other.GetComponent<Damageable>();

        if (damageable == null)
        {
            return;
        }

        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        var msg = new Damageable.DamageMessage()
        {
            amount = damageAmount,

            damager = this,
            direction = (transform.position - other.transform.position).normalized,
            damageSource = transform.position,
            stopCamera = stopCamera
        };

        damageable.ApplyDamage(msg);
        OnAttack.Invoke();
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
            // ĸ�� �ݶ��̴��� ��ġ�� ũ�⸦ �����ɴϴ�.
            Vector3 center = collider.bounds.center;
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius;

            // ĸ�� �ݶ��̴��� ���⿡ ���� ĸ���� ���� ��ġ�� �����մϴ�.
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

            // �ݱ� �� �Ǹ��� �κ��� �׸��ϴ�.
            Gizmos.DrawSphere(bottomSphereCenter, radius);
            Gizmos.DrawSphere(topSphereCenter, radius);
            Gizmos.DrawLine(bottomSphereCenter + radius * Vector3.forward, topSphereCenter + radius * Vector3.forward);
            Gizmos.DrawLine(bottomSphereCenter - radius * Vector3.forward, topSphereCenter - radius * Vector3.forward);
            Gizmos.DrawLine(bottomSphereCenter + radius * Vector3.right, topSphereCenter + radius * Vector3.right);
            Gizmos.DrawLine(bottomSphereCenter - radius * Vector3.right, topSphereCenter - radius * Vector3.right);
        }
    }
}