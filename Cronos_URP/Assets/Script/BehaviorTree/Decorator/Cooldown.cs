using UnityEngine;

/// <summary>
/// 지정된 시간 동안 자식 노드를 실행하지 않고 쿨다운을 적용하는 노드입니다.
/// </summary>
public class Cooldown : DecoratorNode
{
    public float duration = 1.0f;
    float _startTime;

    protected override void OnStart()
    {
        _startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float elapsed = Time.time - _startTime;

        if (elapsed < duration)
        {
            return State.Running;
        }

        return child.Update();
    }
}
