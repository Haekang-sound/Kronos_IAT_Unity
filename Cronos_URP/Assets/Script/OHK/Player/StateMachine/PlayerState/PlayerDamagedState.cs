using UnityEngine;

// 맞았을 때
public class PlayerDamagedState : PlayerBaseState
{
	AnimatorStateInfo stateInfo;
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		// TPCPHUD 인스턴스 받아와서 색/크기 변경
		// 잠시 빼놓음
		UI_TPCPHUD hud = UI_TPCPHUD.GetInstance();
		//hud.ChangeRed();
	}
	public override void Tick(){}
	public override void FixedTick()
	{
		Float();
	}
	public override void LateTick(){}
	public override void Exit(){}

}
