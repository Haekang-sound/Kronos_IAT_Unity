 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// �̵������� �ൿ�� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class MoveTreeBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
		_stateMachine.autoTargetting.target = null;
		animator.ResetTrigger(PlayerHashSet.Instance.Dodge);
		animator.ResetTrigger(PlayerHashSet.Instance.Respawn);
		animator.ResetTrigger(PlayerHashSet.Instance.Death);
	}
}
