
/// <summary>
///행동 트리에서 트리의 시작 노드를 나타냅니다.
/// 이 노드는 단 하나의 자식 노드를 가지며, 
/// 자식 노드의 업데이트를 호출하여 행동 트리를 시작합니다.
/// </summary>
public class Start : Node
{
    public Node child;
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    { 
        return child.Update();
    }

    public override Node Clone()
    {
        Start node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
