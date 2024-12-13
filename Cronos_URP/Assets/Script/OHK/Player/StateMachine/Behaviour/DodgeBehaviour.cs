using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

/// <summary>
/// 회피공격 동작시 행동을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class DodgeBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;
	Vector3 direction;
	[SerializeField] float moveForce;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.autoTargetting.enabled = false;
		SoundManager.Instance.PlaySFX("Player_Dodge_Sound_SE", Player.Instance.transform);

		// 상태전환
		if (_stateMachine.InputReader.moveComposite.magnitude != 0)
		{
			_stateMachine.Rigidbody.rotation = Quaternion.LookRotation(_stateMachine.velocity);
		}

		animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		PlayerStateMachine.GetInstance().SwitchState(new PlayerDodgeState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().autoTargetting.target = null;
		PlayerStateMachine.GetInstance().Player.damageable.enabled = false;
		PlayerStateMachine.GetInstance().MoveForce = moveForce;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player.damageable.enabled = true;
		_stateMachine.autoTargetting.enabled = true;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
