using UnityEngine;

/// <summary>
/// Player�� �������¸� �����ϴ� Ŭ����
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
