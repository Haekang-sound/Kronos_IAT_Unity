using UnityEngine;

/// <summary>
/// 자식 노드 중에서 랜덤으로 하나를 선택하여 실행하는 노드입니다.
/// </summary>
public class RandomSelector : CompositeNode
{
    protected int current;

    protected override void OnStart()
    {
        current = Random.Range(0, children.Count);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        return child.Update();
    }
}