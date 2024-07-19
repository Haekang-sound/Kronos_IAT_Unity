using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class DodgeBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	Vector3 direction;
	private readonly int moveHash = Animator.StringToHash("isMove");
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
		stateMachine = PlayerStateMachine.GetInstance();
		// 상태전환
		PlayerStateMachine.GetInstance().SwitchState(new PlayerParryState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = true;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
// 	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
// 	{
// 
// 
// 	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
