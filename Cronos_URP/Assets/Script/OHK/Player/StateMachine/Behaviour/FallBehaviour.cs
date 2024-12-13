using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ���� ���۽� �ൿ�� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class FallBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerFallState(_stateMachine));
		PlayerStateMachine.GetInstance().autoTargetting.target = null;
		PlayerStateMachine.GetInstance().Rigidbody.useGravity = true;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_stateMachine.GroundChecker.isActiveAndEnabled)
		{
			animator.SetBool(PlayerHashSet.Instance.IsFalling, false);
		}
	}
}
