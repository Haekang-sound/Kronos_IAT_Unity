using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeStopState : PlayerBaseState
{
    public PlayerTimeStopState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.Rigidbody.velocity = Vector3.zero;
    }
    public override void Tick() { }
    public override void FixedTick() { Float(); }
    public override void LateTick() { }
    public override void Exit() { }



}

