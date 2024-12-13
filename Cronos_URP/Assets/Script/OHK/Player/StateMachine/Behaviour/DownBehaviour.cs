using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ٿ���½� �ൿ�� ���� �ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class DownBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerDownState(_stateMachine));
	}
}
