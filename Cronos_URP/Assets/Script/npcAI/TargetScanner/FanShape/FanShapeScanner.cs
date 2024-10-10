using UnityEngine;
using static UnityEngine.Advertisements.Advertisement;

// enemy ��ü�� �÷��̸� ã�� �����ϴ� �� ���ȴ�.
[System.Serializable]
public class FanShapeScanner: MonoBehaviour
{
    public float heightOffset;
    public float detectionRadius;
    [Range(0.0f, 360.0f)]
    public float detectionAngle;
    public float maxHeightDifference;
    public LayerMask viewBlockerLayerMask;

    public GameObject target { private get; set; }

    void OnEnable()
    {
        target = Player.Instance.gameObject;
    }

    /// <summary>
    ///  �Ű������� ���� �÷��̾ ǥ�õǴ��� Ȯ���Ѵ�.
    /// </summary>
    /// <param name="detector">������ ������ ��ü�� Ʈ������.</param>
    /// /// <param name="useHeightDifference">If��꿡�� ���� ���̸� �ִ� ���� ���� ���� ���ؾ� �ϴ��� �ƴϸ� �����ؾ� �ϴ���.</returns>
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
