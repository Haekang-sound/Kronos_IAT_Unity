using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		if (stateMachine.Velocity.magnitude != 0f)
		{
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
		}
	}
	public override void Tick() { }
	public override void FixedTick()
	{
		bool isOnSlope = IsOnSlope();
		if (isOnSlope)
		{
			stateMachine.Rigidbody.useGravity = false;
		}
		else
		{
			stateMachine.Rigidbody.useGravity = true;
		}
		Vector3 velocity = isOnSlope ? AdjustDirectionToSlope(stateMachine.transform.forward) : stateMachine.transform.forward;
		Vector3 gravity = isOnSlope ? Vector3.zero : Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);

		stateMachine.Rigidbody.velocity = velocity * stateMachine.MoveForce + gravity;

	}
	public override void LateTick() { }

	public override void Exit() { }
}
