using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerEnforcedAttackState : PlayerBaseState
{
	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick() { }
	public override void FixedTick() { }
	public override void LateTick() { }
	public override void Exit()
	{
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}
