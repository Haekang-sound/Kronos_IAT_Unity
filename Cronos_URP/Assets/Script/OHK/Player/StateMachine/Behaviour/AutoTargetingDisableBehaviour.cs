using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ÿ���� ������ 
/// �ִϸ��̼Ǵ������� �����ϱ� ���� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class AutoTargetingDisableBehaviour : StateMachineBehaviour
{
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().autoTargetting.enabled = false;
		PlayerStateMachine.GetInstance().autoTargetting.sphere.enabled = false;
	}
}
