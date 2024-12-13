
/// <summary>
/// 자식 노드들을 순차적으로 실행하여 
/// 첫 번째 성공 또는 실행 중인 상태를 반환하는 노드입니다.
/// </summary>
public class Selector : CompositeNode
{
    protected int current;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        for (int i = current; i < children.Count; ++i)
        {
            current = i;
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }
}