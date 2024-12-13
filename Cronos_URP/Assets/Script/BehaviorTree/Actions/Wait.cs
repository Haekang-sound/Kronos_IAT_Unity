
/// <summary>
/// 지정된 대기 시간이 경과하면 성공 상태를 반환하는 액션 노드입니다.
/// </summary>
public class Wait : ActionNode
{
    public float duration = 1f;
    float _elapse;

    protected override void OnStart()
    {
        _elapse = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        _elapse += blackboard.bulletTimeScalable.GetDeltaTime();

        if (_elapse > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
