using Cinemachine;
using System.Resources;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
	private readonly int FallHash = Animator.StringToHash("isFalling");	// ��ȯ�� �ִϸ��̼��� �ؽ�

	public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Velocity.y = 0f;
		stateMachine.Animator.SetBool(FallHash, true);
	}
	public override void Tick()
	{
		if (stateMachine.GroundChecker.IsGrounded())
		{
			stateMachine.Animator.SetBool(FallHash, false);
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
