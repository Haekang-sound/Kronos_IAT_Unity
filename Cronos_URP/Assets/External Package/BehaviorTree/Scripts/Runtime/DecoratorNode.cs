using UnityEngine;

/// <summary>
/// �ൿ Ʈ������ ���Ǵ� ��� Ŭ�����Դϴ�.
/// �ϳ��� �ڽ� ��带 �����ϴ�.
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
