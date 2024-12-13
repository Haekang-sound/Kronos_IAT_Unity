using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GuardBehavior : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerGuardState(_stateMachine));
		Player.Instance.SetUseKnockback(true);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!animator.IsInTransition(layerIndex))
		{
			_stateMachine.Player.EndParry();
		}

		// �̵�Ű �Է¿��θ� Ȯ���ϰ�
		// ������ �����Ѵ�.
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player.Instance.SetUseKnockback(false);
	}
}
