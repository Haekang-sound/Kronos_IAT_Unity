
/// <summary>
/// 자식 노드의 상태를 반전시키는 노드입니다.
/// 자식 노드가 성공일 경우 실패를, 실패일 경우 성공을 반환합니다.
/// </summary>
public class Inverter : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Success;
            case State.Success:
                return State.Failure;
        }
        return State.Failure;
    }
}