using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		stateMachine.AutoTargetting.Target = null;
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// �̵�Ű�Է��� ������
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵���
			animator.SetBool(moveHash, true);
		}
		//bool test = Input.GetKeyUp(KeyCode.Mouse1);
		if (!Input.GetKey(KeyCode.Mouse1))
		{
			animator.SetBool(guradHash, false);
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
