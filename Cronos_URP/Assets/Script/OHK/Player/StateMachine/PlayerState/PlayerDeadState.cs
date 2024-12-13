using UnityEngine;

/// <summary>
/// Player의 죽음상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerDeadState : PlayerBaseState
{
	public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		_stateMachine.Animator.Rebind();
	}

	public override void Tick() { }
	public override void FixedTick() { }
	public override void LateTick() { }
	public override void Exit() { }
}
