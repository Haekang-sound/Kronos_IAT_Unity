using UnityEngine;

/// <summary>
/// Player의 피격상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerDamagedState : PlayerBaseState
{
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter(){}
	public override void Tick(){}
	public override void FixedTick()
	{
		Float();
	}
	public override void LateTick(){}
	public override void Exit(){}

}
