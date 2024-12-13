
/// <summary>
/// 클래스는 Blackboard에서 타겟이 존재하는지 확인하는 행동 노드입니다.
/// 타겟이 null인 경우 성공 상태를 반환하고, 
/// 타겟이 존재하는 경우 실패 상태를 반환합니다.
/// </summary>
class IsTargetNull : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.target == null)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}