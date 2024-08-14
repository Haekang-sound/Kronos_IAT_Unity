using UnityEngine;

public class PlayerBuffState : PlayerBaseState
{

	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{

	}
	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
	}
	public override void Exit()
	{
	}


}
