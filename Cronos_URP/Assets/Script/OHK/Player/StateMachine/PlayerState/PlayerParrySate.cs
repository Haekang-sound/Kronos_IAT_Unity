using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int attackHash = Animator.StringToHash("Attack");
	Vector3 totalMove;
	public override void Enter()
	{
		stateMachine.InputReader.onLAttackStart += Attack;
		if (stateMachine.Velocity.magnitude != 0f)
		{
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
		}
	}
	public override void Tick()
	{
		Vector3 rootMotion = stateMachine.Animator.deltaPosition;
		rootMotion.y = 0;
		totalMove += rootMotion;
	}
	public override void FixedTick()
	{
		if (IsOnSlope())
		{
			stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(totalMove) * stateMachine.MoveForce;
		}
		else
		{
			stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(totalMove) * stateMachine.MoveForce;

		}
		Float();
		totalMove = Vector3.zero;
	}
	public override void LateTick() { }

	public override void Exit()
	{
		stateMachine.InputReader.onLAttackStart += Attack;
	}
	private void Attack()
	{
		stateMachine.AutoTargetting.AutoTargeting();
		PlayerStateMachine.GetInstance().Animator.SetBool(attackHash, true);
	}
}
