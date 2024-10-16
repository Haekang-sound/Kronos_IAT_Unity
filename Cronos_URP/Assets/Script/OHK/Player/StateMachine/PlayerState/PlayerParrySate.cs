using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int attackHash = Animator.StringToHash("Attack");
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
		if (stateMachine.MoveForce > 1f)
		{
			stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce;
		}
		else
		{
			stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(stateMachine.Animator.deltaPosition / Time.deltaTime);
		}
	}
	public override void FixedTick()
	{
		Float();
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
