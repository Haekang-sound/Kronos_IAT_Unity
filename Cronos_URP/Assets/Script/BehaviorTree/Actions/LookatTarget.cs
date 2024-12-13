using UnityEngine;

/// <summary>
/// ����� �־��� ���� �̳��� ������ ���� ���¸� ��ȯ�ϸ�,
/// �׷��� ������ ȸ���Ͽ� �ٶ󺾴ϴ�.
/// </summary>
public class LookatTarget : ActionNode
{
    public float rotationSpeed = 1;
    public float angleThreshold = 10.0f; // �ٶ󺸴� ������ ������ �ִ� ����

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

        // �� ���� ���� ���� ���
        float angle = Vector3.Angle(forward, toTarget);

        // ������ �Ӱ谪 �����̸� true ��ȯ
        return angle <= angleThreshold;
    }

    public void LookAtTarget()
    {
        var target = blackboard.target;
        if (target == null) return;

        // �ٶ󺸴� ���� ����
        Transform transform = context.transform;
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
