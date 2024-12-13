
/// <summary>
/// 자식 노드들을 순차적으로 실행하여, 
/// 모든 자식 노드가 성공하면 성공 상태를 반환하고, 
/// 실패하면 실패 상태를 반환하는 노드입니다.
/// </summary>
public class Sequencer : CompositeNode
{
    int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];

        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}
