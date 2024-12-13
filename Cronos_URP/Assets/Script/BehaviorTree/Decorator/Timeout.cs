using UnityEngine;

/// <summary>
/// 자식 노드의 실행 시간에 제한을 두는 노드입니다.
/// 지정된 시간 `duration`을 초과하면 자식 노드의 실행을 실패로 처리하고, 그 이내에는 자식 노드를 실행합니다.
/// </summary>

public class Timeout : DecoratorNode
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

        if (elapsed > duration)
        {
            return State.Failure;
        }

        return child.Update();
    }
}