using UnityEngine;

/// <summary>
/// 적이 특정 구역 내에서 플레이어를 찾고 추적하는 데 사용되는 스캐너 클래스입니다. 
/// 이 클래스는 플레이어의 위치를 감지하고, 장애물에 의해 가려지지 않는지 확인하며, 
/// 플레이어가 감지 범위 내에 있을 경우 이를 반환합니다.
/// </summary>
[System.Serializable]
public class FanShapeScanner : MonoBehaviour
{
    [SerializeField]
    private bool drawGizmos = true;

    public float heightOffset;
    public float detectionRadius;
    [Range(0.0f, 360.0f)]
    public float detectionAngle;
    public float maxHeightDifference;
    public LayerMask viewBlockerLayerMask;

    [Header("Target Setting")]
    [SerializeField]
    private GameObject _target;
    public GameObject target
    {
        private get => _target;
        set => _target = value;
    }

    void OnEnable()
    {
        if (target == null)
        {
            target = Player.Instance.gameObject;
        }
    }

    /// <summary>
    /// 주어진 매개변수에 따라 플레이어가 표시되는지 확인하고, 타겟을 반환합니다.
    /// </summary>
    /// <param name="detector">감지를 실행할 객체의 트랜스폼.</param>
    /// <param name="useHeightDifference">높이 차이를 고려할지 여부.</param>
    /// <returns>타겟이 감지되면 타겟 객체를 반환, 그렇지 않으면 null.</returns>
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

    public void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        EditorGizmo(gameObject.transform);
    }

    public void EditorGizmo(Transform transform)
    {
        Color c = new Color(0, 0, 0.7f, 0.1f);

        UnityEditor.Handles.color = c;
        Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, 0.2f);
    }

#endif
}
