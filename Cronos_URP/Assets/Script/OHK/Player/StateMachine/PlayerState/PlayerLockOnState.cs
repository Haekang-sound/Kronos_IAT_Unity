using UnityEditor;
using UnityEngine;

/// <summary>
/// 락온을 상태를 정의하는 클래스
/// (현재 사용하지 않음)
/// </summary>
public class PlayerLockOnState : PlayerBaseState
{
 	public PlayerLockOnState(PlayerStateMachine stateMachine) : base(stateMachine) { }
 	public override void Enter(){}
 	public override void Tick(){}
 	public override void FixedTick(){}
 	public override void LateTick(){}
 	public override void Exit(){}

}
