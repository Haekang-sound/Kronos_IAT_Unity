using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleDamager : MonoBehaviour
{
    public bool drawGizmos;

    public float damageAmount = 1;
    public bool stopCamera = false;

    public LayerMask targetLayers;

    protected GameObject m_owner;
    protected bool m_inAttack = false;

    public delegate void TriggerEnterAction(Collider other);
    public event TriggerEnterAction OnTriggerEnterEvent;

    SoundManager soundManager;

    private void OnEnable()
    {
        soundManager = SoundManager.Instance;
    }

    public void SetOwner(GameObject owner)
    {
        m_owner = owner;
    }

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
        //GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (m_inAttack)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);

            Gizmos.matrix = transform.localToWorldMatrix;

            var collider = GetComponent<Collider>();

            if (collider is BoxCollider boxCollider)
            {
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                // ĸ���� �������� ���̸� ������
                float radius = capsuleCollider.radius;
                float height = capsuleCollider.height / 2.0f - radius;

                // ĸ���� ������ ����
                Vector3 direction = Vector3.up;
                if (capsuleCollider.direction == 0) // X-axis
                {
                    direction = Vector3.right;
                }
                else if (capsuleCollider.direction == 2) // Z-axis
                {
                    direction = Vector3.forward;
                }

                // ĸ���� �� ������ ���
                Vector3 offset = direction * height;
                Vector3 topSphereCenter = capsuleCollider.center + offset;
                Vector3 bottomSphereCenter = capsuleCollider.center - offset;

                // ���� �ݱ��� �׸�
                Gizmos.DrawSphere(topSphereCenter, radius);

                // �Ʒ��� �ݱ��� �׸�
                Gizmos.DrawSphere(bottomSphereCenter, radius);

                // �� �ݱ� ���̿� �ڽ��� �׸�
                Vector3 boxCenter = (topSphereCenter + bottomSphereCenter) / 2;
                Vector3 boxSize = new Vector3(
                    direction == Vector3.right ? height + radius : radius,
                    direction == Vector3.up ? height + radius : radius,
                    direction == Vector3.forward ? height + radius : radius
                );
                Gizmos.DrawCube(boxCenter, boxSize);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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

        if (damageable.gameObject == m_owner)
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
            direction = Vector3.up,
            stopCamera = stopCamera
        };

        damageable.ApplyDamage(msg);
    }
}