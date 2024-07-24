using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

// enemy 객체가 플레이를 찾고 추적하는 데 사용된다.
[System.Serializable]
public class TargetScanner
{
    public float heightOffset;
    public float detectionRadius;
    [Range(0.0f, 360.0f)]
    public float detectionAngle;
    public float maxHeightDifference;
    public LayerMask viewBlockerLayerMask;

    public GameObject target { private get; set; }

    /// <summary>
    ///  매개변수에 따라 플레이어가 표시되는지 확인한다.
    /// </summary>
    /// <param name="detector">감지를 실행할 객체의 트랜스폼.</param>
    /// /// <param name="useHeightDifference">If계산에서 높이 차이를 최대 높이 차이 값과 비교해야 하는지 아니면 무시해야 하는지.</returns>
    public GameObject Detect(Transform detector, bool useHeightDifference = true)
    {
        if (target == null)
        {
            return null;
        }

        Vector3 eyePos = detector.position + Vector3.up * heightOffset;
        Vector3 toPlayer = target.transform.position - eyePos;
        Vector3 toPlayerTop = target.transform.position + Vector3.up * 1.5f - eyePos;

        if (useHeightDifference && Mathf.Abs(toPlayer.y + heightOffset) > maxHeightDifference)
        {
            return null;
        }

        Vector3 toPlayerFlat = toPlayer;
        toPlayerFlat.y = 0;

        if (toPlayerFlat.sqrMagnitude <= detectionRadius * detectionRadius)
        {
            if (Vector3.Dot(toPlayerFlat.normalized, detector.forward) >
                Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {

                bool canSee = false;

                Debug.DrawRay(eyePos, toPlayer, Color.blue);
                Debug.DrawRay(eyePos, toPlayerTop, Color.blue);

                canSee |= !Physics.Raycast(eyePos, toPlayer.normalized, detectionRadius,
                    viewBlockerLayerMask, QueryTriggerInteraction.Ignore);

                canSee |= !Physics.Raycast(eyePos, toPlayerTop.normalized, toPlayerTop.magnitude,
                    viewBlockerLayerMask, QueryTriggerInteraction.Ignore);

                if (canSee)
                    return target;
            }
        }

        return null;
    }

#if UNITY_EDITOR

    public void EditorGizmo(Transform transform)
    {
        Color c = new Color(0, 0, 0.7f, 0.3f);

        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, 0.2f);
    }

#endif
}
