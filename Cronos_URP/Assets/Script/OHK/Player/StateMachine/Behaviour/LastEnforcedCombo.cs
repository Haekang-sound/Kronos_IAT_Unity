using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEnforcedCombo : StateMachineBehaviour
{
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int chargeHash = Animator.StringToHash("Charge");
	private readonly int chargeAttackHash = Animator.StringToHash("chargeAttack");
	private readonly int guradHash = Animator.StringToHash("isGuard");
	public float hitStopTime;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().HitStop.hitStopTime = hitStopTime;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			animator.SetBool(guradHash, true);
		}
		// �̵�Ű�Է��� ������
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			// �̵���
			animator.SetBool(moveHash, true);
		}
		else// Ȥ��
		{
			// �̵��ƴ�
			animator.SetBool(moveHash, false);
		}

		// ��Ŭ�� ������ �߿��� ��¡
		if (Input.GetKey(KeyCode.Mouse0))
		{
			float current = animator.GetFloat(chargeHash);
			animator.SetFloat(chargeHash, current + Time.deltaTime);
		}

		// ������������ ��¡���̴�
		if (Input.GetKey(KeyCode.Mouse0))
		{
			//��ǲ�߿� ����� ��������ҵ�
			animator.SetBool(chargeAttackHash, true);
		}
		else
		{
			//��ǲ�߿� ����� ��������ҵ�
			animator.SetBool(chargeAttackHash, false);
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
