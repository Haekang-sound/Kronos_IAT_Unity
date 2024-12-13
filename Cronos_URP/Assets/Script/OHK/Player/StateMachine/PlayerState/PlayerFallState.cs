using Cinemachine;
using System.Resources;
using UnityEngine;

/// <summary>
/// Player의 낙하상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerFallState : PlayerBaseState
{
	public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		_stateMachine.velocity.y = 0f;
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsFalling, true);
	}

	public override void Tick()
	{
		if (_stateMachine.GroundChecker.IsGrounded())
		{
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsFalling, false);
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
