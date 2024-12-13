
/// <summary>
/// 자식 노드 중 하나의 상태가 변경되면 실행 중인 노드를 중단하는 기능을 수행하는 액션 노드입니다.
/// </summary>
public class InterruptSelector : Selector
{
    protected override State OnUpdate()
    {
        int previous = current;
        base.OnStart();
        var status = base.OnUpdate();

        if (previous != current)
        {
            if (children[previous].state == State.Running)
            {
                children[previous].Abort();
            }
        }

        return status;
    }
}