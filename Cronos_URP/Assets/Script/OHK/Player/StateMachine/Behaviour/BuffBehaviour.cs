using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����� �����ϱ� ���� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class BuffBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player.Instance.isBuff = true;
		Player.Instance.IsEnforced = true;
		EffectManager.Instance.SwordAuraOn();

		PlayerStateMachine.GetInstance().SwitchState(new PlayerBuffState(PlayerStateMachine.GetInstance()));
	}
}
