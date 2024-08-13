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
        Vector3 rootMotion = stateMachine.Animator.deltaPosition;
        rootMotion.y = 0;
        if (IsOnSlope())
        {
            stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(rootMotion) * stateMachine.MoveForce;
        }
        else
        {
            stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(rootMotion) * stateMachine.MoveForce;

        }

	}
	public override void LateTick() { }

	public override void Exit() { }
}
