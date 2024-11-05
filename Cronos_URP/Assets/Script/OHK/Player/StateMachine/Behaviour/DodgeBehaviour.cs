using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DodgeBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	Vector3 direction;
		[SerializeField] float moveForce;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.AutoTargetting.enabled = false;
		SoundManager.Instance.PlaySFX("Player_Dodge_Sound_SE", Player.Instance.transform);
		// 상태전환
		stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
		animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		PlayerStateMachine.GetInstance().SwitchState(new PlayerDodgeState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().AutoTargetting.Target = null;
		PlayerStateMachine.GetInstance().Player._damageable.enabled = false;
		PlayerStateMachine.GetInstance().MoveForce = moveForce;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
 	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 	{
		if ( stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex) )
		{
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity); 
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player._damageable.enabled = true;
		stateMachine.AutoTargetting.enabled = true;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    // Implement code that processes and affects root motion
	}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
