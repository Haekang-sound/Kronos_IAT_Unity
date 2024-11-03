using Cinemachine;
using System.Resources;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
	public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Velocity.y = 0f;
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.isFalling, true);
	}
	public override void Tick()
	{
		if (stateMachine.GroundChecker.IsGrounded())
		{
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.isFalling, false);
		}
		CalculateMoveDirection();
	}
	public override void FixedTick()
	{
		FaceMoveDirection();
		Move();
	}
	public override void LateTick(){}
	public override void Exit(){}

}
