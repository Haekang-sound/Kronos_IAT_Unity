using UnityEngine;

// 맞았을 때
public class PlayerDamagedState : PlayerBaseState
{
	AnimatorStateInfo stateInfo;
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public static readonly int hashDamageBase = Animator.StringToHash("Damage_Head");
	public override void Enter()
	{
		// TPCPHUD 인스턴스 받아와서 색/크기 변경
		UI_TPCPHUD hud = UI_TPCPHUD.GetInstance();
		hud.ChangeRed();

		stateMachine.Animator.SetTrigger("Damaged");
	}
	public override void Tick(){}
	public override void FixedTick(){}
	public override void LateTick(){}
	public override void Exit(){}

}
