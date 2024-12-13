/// <summary>
/// 플레이어의 다운상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerDownState : PlayerBaseState
{
	public PlayerDownState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter() { }
	public override void Exit() { }
	public override void FixedTick() { Float(); }
	public override void LateTick() { }
	public override void Tick() { }
}