using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 강화공격 이펙트를 On/Off 하는 클래스
/// 
/// ohk    v1
/// </summary>
public class SwordAuraBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		EffectManager.Instance.SwordAuraOn();
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		Player.Instance.IsEnforced = false;
		EffectManager.Instance.SwordAuraOff();
	}
}
