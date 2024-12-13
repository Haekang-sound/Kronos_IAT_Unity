using UnityEngine;

/// <summary>
/// 대상이 주어진 각도 이내에 있으면 성공 상태를 반환하며,
/// 그렇지 않으면 회전하여 바라봅니다.
/// </summary>
public class LookatTarget : ActionNode
{
    public float rotationSpeed = 1;
    public float angleThreshold = 10.0f; // 바라보는 것으로 간주할 최대 각도

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var target = blackboard.target;
        if (target == null) return State.Failure;


        if (IsLookAtTarget())
        {
            return State.Success;
        }
        else
        {
            LookAtTarget();
        }

        return State.Running;
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (context == null) return;

        var c = Color.blue;
        c.a = 0.3f;
        UnityEditor.Handles.color = c;

        Vector3 rotatedForward = Quaternion.Euler(0, -angleThreshold * 0.5f, 0) * context.agent.transform.forward;
        UnityEditor.Handles.DrawSolidArc(context.agent.transform.position, Vector3.up, rotatedForward, angleThreshold, 20);
#endif
    }

    public bool IsLookAtTarget()
    {
        var target = blackboard.target;
        if (target == null) return false;

        Transform transform = context.transform;
        Vector3 forward = transform.forward;
        Vector3 toTarget = (target.transform.position - transform.position).normalized;

        // 두 벡터 간의 각도 계산
        float angle = Vector3.Angle(forward, toTarget);

        // 각도가 임계값 이하이면 true 반환
        return angle <= angleThreshold;
    }

    public void LookAtTarget()
    {
        var target = blackboard.target;
        if (target == null) return;

        // 바라보는 방향 설정
        Transform transform = context.transform;
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
