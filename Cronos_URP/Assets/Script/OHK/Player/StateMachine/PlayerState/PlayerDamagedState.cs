using UnityEngine;

// �¾��� ��
public class PlayerDamagedState : PlayerBaseState
{
	AnimatorStateInfo stateInfo;
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		// TPCPHUD �ν��Ͻ� �޾ƿͼ� ��/ũ�� ����
		// ��� ������
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
