using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }
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
		if (stateMachine.Velocity.magnitude != 0f)
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
		stateMachine.InputReader.onLAttackStart -= Attack;
	}
	private void Attack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.Attack, true);
	}
}
