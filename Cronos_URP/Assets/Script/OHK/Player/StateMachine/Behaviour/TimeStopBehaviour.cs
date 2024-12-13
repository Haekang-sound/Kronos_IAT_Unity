using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ð����������� �����ϴ� Ŭ����
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
