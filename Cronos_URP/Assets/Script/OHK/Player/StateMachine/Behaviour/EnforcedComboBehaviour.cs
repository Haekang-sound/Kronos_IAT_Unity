using UnityEngine;

public class EnforcedComboBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	private readonly int attackHash = Animator.StringToHash("Attack");
	private readonly int moveHash = Animator.StringToHash("isMove");
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		animator.ResetTrigger(attackHash);
		animator.SetBool("NextCombo", false);
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

		if (Input.GetKey(KeyCode.Mouse0))
		{
			float current = animator.GetFloat("Charge");
			animator.SetFloat("Charge", current + Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			animator.SetBool("NextCombo", true);
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			animator.SetFloat("Charge", 0);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			// nextcombo 가 예정되어있다면
			if (animator.GetBool("NextCombo"))
			{
				// 리턴
				return;
			}
			else // 그렇지 않다면
			{
				//PlayerStateMachine.GetInstance().SwitchState(new PlayerMoveState(PlayerStateMachine.GetInstance()));
				return;
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

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
