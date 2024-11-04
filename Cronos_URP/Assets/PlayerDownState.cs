public class PlayerDownState : PlayerBaseState
{
	public PlayerDownState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
	}

	public override void Exit()
	{
	}

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick()
	{
	}

	public override void Tick()
	{
	}
}