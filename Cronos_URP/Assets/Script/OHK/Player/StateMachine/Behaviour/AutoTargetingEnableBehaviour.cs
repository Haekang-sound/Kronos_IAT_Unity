using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ÿ���� ����� 
/// �ִϸ��̼Ǵ������� �����ϱ� ���� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class AutoTargetingEnableBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		PlayerStateMachine.GetInstance().autoTargetting.enabled = true;
		PlayerStateMachine.GetInstance().autoTargetting.sphere.enabled = true;
	}
}
