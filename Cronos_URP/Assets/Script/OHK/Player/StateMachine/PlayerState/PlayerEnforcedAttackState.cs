using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Player의 강화공격상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerEnforcedAttackState : PlayerBaseState
{
	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		_stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		_stateMachine.Rigidbody.velocity = Vector3.zero;
	}

	public override void FixedTick() { }
	public override void Tick() { }
	public override void LateTick() { }

	public override void Exit()
	{
		_stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
	}

	private void SwitchToDefanceState()
	{
		_stateMachine.SwitchState(new PlayerGuardState(_stateMachine));
	}
}
