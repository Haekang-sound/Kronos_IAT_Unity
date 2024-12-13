using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȭ���� ����Ʈ�� On/Off �ϴ� Ŭ����
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
