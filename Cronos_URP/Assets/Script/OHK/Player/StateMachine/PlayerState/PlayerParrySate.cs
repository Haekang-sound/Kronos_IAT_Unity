using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
	}
	public override void Tick(){}
	public override void FixedTick(){}
	public override void LateTick(){}

	public override void Exit(){}
}
