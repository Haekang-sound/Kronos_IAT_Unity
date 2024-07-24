using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Splashdamage : MonoBehaviour
{
    public float damageAmount = 1f;
    public float radious = 1f;
    public float yOffset = 1f;
    public LayerMask targetLayers;
    public GameObject m_owner;

    public float attackTime = 1.75f;
    private float passedTime = 0;

    private void Update()
    {
        if (UpdateTimers())
        {
            CheckForDamageable();
        }
    }

    private bool UpdateTimers()
    {
        passedTime -= Time.deltaTime;

        if (passedTime < 0)
        {
            passedTime = attackTime;
            return true;
        }

        return false;
    }

    private void CheckForDamageable()
    {
        var pos = transform.position;
        pos.y += yOffset;
        Collider[] colliders = Physics.OverlapSphere(pos, radious);

        foreach (Collider c in colliders)
        {
            if ((targetLayers.value & (1 << c.gameObject.layer)) == 0)
            {
                continue;
            }

            var damageable = c.GetComponent<Damageable>();

            if (damageable == null)
            {
                continue;
            }

            if (damageable.gameObject == m_owner)
            {
                continue;
            }

            var msg = new Damageable.DamageMessage()
            {
                amount = damageAmount,
                damager = this,
                direction = Vector3.up,
                stopCamera = false
            };

            damageable.ApplyDamage(msg);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.6f);

        var pos = transform.position;
        pos.y += yOffset;

        Gizmos.DrawSphere(pos, radious);
    }
#endif
}
