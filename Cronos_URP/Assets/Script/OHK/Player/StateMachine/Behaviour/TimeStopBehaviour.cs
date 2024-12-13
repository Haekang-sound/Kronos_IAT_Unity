using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 시간정지동작을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class TimeStopBehaviour : StateMachineBehaviour
{
	private	PlayerStateMachine _stateMachine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.CPBoomb);
		_stateMachine.SwitchState(new PlayerTimeStopState(_stateMachine));

	}
}
