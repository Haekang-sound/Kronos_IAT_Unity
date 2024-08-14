using System.Collections;
using System.Collections.Generic;

//using UnityEditorInternal;
using UnityEngine;

public class BuffBehaviour : StateMachineBehaviour
{
	private readonly int attackHash = Animator.StringToHash("Attack");
	private readonly int BuffHash = Animator.StringToHash("Buff");
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int idleHash = Animator.StringToHash("goIdle");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");
	[SerializeField] private float buffTimer = 0f;
	[SerializeField] private float buffTime;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().SwitchState(new PlayerBuffState(PlayerStateMachine.GetInstance()));
		//PlayerStateMachine.GetInstance().AutoTargetting.Target = null;
		animator.ResetTrigger(attackHash);
		animator.ResetTrigger(idleHash);
		animator.SetBool(BuffHash, true);
		buffTimer = 0f;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		buffTimer += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			animator.SetBool(guradHash, true);
		}

		// 특정 조건을 만족할 때 애니메이션을 종료하고 targetStateName으로 전환
		if (buffTimer > buffTime)
		{
			animator.SetTrigger(idleHash);
		}

		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(moveHash, true);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			animator.SetTrigger(attackHash);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger(dodgeHash);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool(BuffHash, false);
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
