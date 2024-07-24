using Cinemachine;
using System.Resources;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
	private readonly int FallHash = Animator.StringToHash("isFalling");	// 전환될 애니메이션의 해쉬

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
			//stateMachine.Animator.speed = 1f;
			stateMachine.Animator.SetBool(FallHash, false);
			//stateMachine.SwitchState(new PlayerIdleState(stateMachine));
		}

	}
	public override void FixedTick()
	{
		Move();
	}
	public override void LateTick()
	{
	}
	public override void Exit()
	{

	}

}
