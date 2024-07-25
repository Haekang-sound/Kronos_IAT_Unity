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

    [Tooltip("대미지를 입힐 수 있는 각도입니다. 항상 월드 XZ 평면에 있으며, 전방은 hitForwardRoation으로 회전합니다.")]
    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;
    [Tooltip("타격 각도 영역을 정의하는 기준 각도를 회전시킬 수 있습니다.")]
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

        // 데미지를 입은 방향과 데미지 영역을 투영(projection)
        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
        {
            return;
        }

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
