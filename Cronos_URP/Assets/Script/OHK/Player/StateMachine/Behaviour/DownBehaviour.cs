using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다운상태시 행동을 정의 하는 클래스
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
