
/// <summary>
/// 자식 노드를 반복적으로 실행하는 노드입니다.
/// 자식 노드의 상태와 관계없이 계속해서 자식 노드를 실행합니다.
/// </summary>
public class Repeat : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        child.Update();
        return State.Running;
    }
}
