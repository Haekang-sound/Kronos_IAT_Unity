
/// <summary>
/// 자식 노드가 성공 상태일 경우 실패 상태로 변경하는 노드입니다.
/// </summary>
public class Failure : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var state = child.Update();
        if (state == State.Success)
        {
            return State.Failure;
        }
        return state;
    }
}