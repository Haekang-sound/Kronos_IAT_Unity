using UnityEngine;

/// <summary>
/// 지정된 확률에 따라 실패 상태를 반환하는 액션 노드입니다.
/// </summary>
public class RandomFailure : ActionNode
{
    [Range(0, 1)]
    public float chanceOfFailure = 0.5f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float value = Random.value;
        if (value > chanceOfFailure)
        {
            return State.Failure;
        }
        return State.Success;
    }
}