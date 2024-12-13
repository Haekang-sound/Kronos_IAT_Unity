using UnityEngine;

/// <summary>
/// 행동 트리에서 사용되는 노드 클래스입니다.
/// 하나의 자식 노드를 갖습니다.
/// </summary>
public abstract class DecoratorNode: Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
