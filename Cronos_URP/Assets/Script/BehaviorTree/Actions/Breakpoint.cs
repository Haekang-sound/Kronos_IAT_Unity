using UnityEngine;

/// <summary>
/// 디버깅을 위한 중단점을 설정하는 행동 노드입니다.
/// 이 노드는 실행 시 'Debug.Break()'를 호출하여 중단점을 트리거하고, 
/// 성공 상태를 반환합니다.
/// </summary>
public class Breakpoint : ActionNode
{
    protected override void OnStart()
    {
        Debug.Log("Trigging Breakpoint");
        Debug.Break();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
