using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackVBehaviour : StateMachineBehaviour
{
	// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		/// R������ ������ ���¶�� ���� �˸���.
		bool isVariation = animator.GetBool("ComAttackVariation");

		// Ȱ��ȭ �Ǿ��ٸ�
		if (isVariation)
		{
			// ��Ŭ���� rattack���� �ٲ��
			PlayerStateMachine.GetInstance().InputReader.onRAttackStart += RAttack;
			PlayerStateMachine.GetInstance().IsRattack = true;
		}
	}
	public void RAttack()
	{
		Debug.Log("�ٸ������� �ϰھ��");
		PlayerStateMachine.GetInstance().Animator.SetTrigger("Rattack");

	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
	}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateMachineEnter is called when entering a state machine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	//{
	//    
	//}

	// OnStateMachineExit is called when exiting a state machine via its Exit Node
	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
	}
}
