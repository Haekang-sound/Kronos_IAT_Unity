using UnityEngine;

/// <summary>
/// 지정된 거리 내에 타겟이 있는지 또는 없는지를 검사하여, 
/// 그 결과에 따라 실패 또는 성공 상태를 반환합니다.
/// </summary>
class CheckTargetDistance : ActionNode
{
    public enum Comparison
    {
        Greater,
        Less
    }

    public float distance;
    public Comparison comparison;


    protected override void OnStart()
    {
        //_target = blackboard.target;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 toTarget = blackboard.target.transform.position - context.transform.position;
        bool checkDistance = toTarget.sqrMagnitude < distance * distance;

        if (comparison == Comparison.Greater && checkDistance == true)
        {
            return State.Failure;
        }
        else if (comparison == Comparison.Less && checkDistance == false)
        {
            return State.Failure;
        }

        return State.Success;
    }
    public override void OnDrawGizmos()
    {
        if(context == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(context.transform.position, distance);
    }
}
