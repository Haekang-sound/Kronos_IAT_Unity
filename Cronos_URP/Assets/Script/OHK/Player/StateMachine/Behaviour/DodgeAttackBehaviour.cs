using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAttackBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	[SerializeField] float moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		
		stateMachine.MoveForce = moveForce;
		//stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.HitStop.hitStopTime = hitStopTime;
		stateMachine.Player.DodgeAttack = true;

		animator.ResetTrigger("Attack");

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
// 	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
// 	{
// 	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        stateMachine.Player.DodgeAttack = false;
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
