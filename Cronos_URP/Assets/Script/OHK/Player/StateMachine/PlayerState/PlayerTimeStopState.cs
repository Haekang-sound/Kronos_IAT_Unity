using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 시간정지 상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerTimeStopState : PlayerBaseState
{
    public PlayerTimeStopState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _stateMachine.Rigidbody.velocity = Vector3.zero;
    }

    public override void Tick() { }
    public override void FixedTick() { Float(); }
    public override void LateTick() { }
    public override void Exit() { }
}

