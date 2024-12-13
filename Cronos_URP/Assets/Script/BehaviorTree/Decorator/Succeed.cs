
/// <summary>
/// 자식 노드의 상태가 `Failure`일 때만 `Success`를 반환하는 노드입니다.
/// 자식 노드가 `Success`나 `Running` 상태일 경우, 그 상태를 그대로 반환합니다.
/// </summary>
public class Succeed : DecoratorNode
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
        if (state == State.Failure)
        {
            return State.Success;
        }
        return state;
    }
}