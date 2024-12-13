using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ð����� �غ����� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class TimeSlashReadyBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;
	[SerializeField] public AnimationCurve timeSlashCurve;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerTimeSlashReadyState(_stateMachine));
	}
}
