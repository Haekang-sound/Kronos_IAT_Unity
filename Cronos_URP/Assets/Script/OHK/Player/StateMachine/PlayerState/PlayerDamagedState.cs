using UnityEngine;

/// <summary>
/// Player�� �ǰݻ��¸� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class PlayerDamagedState : PlayerBaseState
{
	public PlayerDamagedState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter(){}
	public override void Tick(){}
	public override void FixedTick()
	{
		Float();
	}
	public override void LateTick(){}
	public override void Exit(){}

}
