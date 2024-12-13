using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프 사용을 관리하기 위한 클래스
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
