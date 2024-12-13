using UnityEngine;

/// <summary>
/// 주어진 메시지를 콘솔에 출력하는 행동 노드입니다.
/// 메시지를 출력하고 나면 성공 상태를 반환합니다.
/// </summary>

public class Log : ActionNode
{
    public string message;

    protected override void OnStart()
    {
        description = message;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Debug.Log($"{message}");
        return State.Success;
    }
}
