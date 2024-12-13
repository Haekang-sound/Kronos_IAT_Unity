using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유휴상태시 동작을 정의하기 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class IdleBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		animator.ResetTrigger(PlayerHashSet.Instance.GoIdle);
		_stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}
		else
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, false);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			animator.SetTrigger(PlayerHashSet.Instance.Attack);
		}

		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			animator.SetBool(PlayerHashSet.Instance.IsGuard, true);
		}
	}
}
