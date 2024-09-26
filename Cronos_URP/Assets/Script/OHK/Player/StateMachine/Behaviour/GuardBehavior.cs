using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GuardBehavior : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int guradHash = Animator.StringToHash("isGuard");

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
		Player.Instance.SetUseKnockback(true);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!animator.IsInTransition(layerIndex))
		{
			stateMachine.Player.EndParry();
		}

		// 이동키입력을 받으면
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(moveHash, true);
		}


	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player.Instance.SetUseKnockback(false);
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
