using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행동 트리에서 사용되는 노드 클래스입니다.
/// 여러 자식 노드를 갖습니다.
/// </summary>
public abstract class CompositeNode : Node
{
    [HideInInspector] public List<Node> children = new List<Node>();

    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }
}
