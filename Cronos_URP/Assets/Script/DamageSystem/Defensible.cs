using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Damageable;

public class Defensible : MonoBehaviour
{
    public bool isDefending;
    [Range(1.0f, 100.0f)]
    public float dampRatio = 10.0f;

    [Tooltip("������� ���� �� �ִ� �����Դϴ�. �׻� ���� XZ ��鿡 ������, ������ hitForwardRoation���� ȸ���մϴ�.")]
    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;
    [Tooltip("Ÿ�� ���� ������ �����ϴ� ���� ������ ȸ����ų �� �ֽ��ϴ�.")]
    [Range(0.0f, 360.0f)]
    [FormerlySerializedAs("hitForwardRoation")] //SHAME!
    public float hitForwardRotation = 360.0f;

    public void ApplyDamage(ref DamageMessage data)
    {
        if (!isDefending)
        {
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        // �������� ���� ����� ������ ������ ����(projection)
        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
        {
            return;
        }

        // ���� ����Ʈ
        Debug.Log("now Guarding");
        EffectManager.Instance.CreateGuardFX();

        data.amount /= dampRatio;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }

        UnityEditor.Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
    }
#endif
}
