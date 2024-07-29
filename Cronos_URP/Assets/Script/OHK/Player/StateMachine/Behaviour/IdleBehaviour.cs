using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	Vector3 direction;

	private readonly int attackHash = Animator.StringToHash("Attack");
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int guradHash = Animator.StringToHash("isGuard");

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		animator.ResetTrigger("goIdle");
		stateMachine.SwitchState(new PlayerIdleState(stateMachine));
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(moveHash, true);
		}
		else
		{
			animator.SetBool(moveHash, false);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			animator.SetTrigger(attackHash);
		}

		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			animator.SetBool(guradHash, true);
		}
	}
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

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
