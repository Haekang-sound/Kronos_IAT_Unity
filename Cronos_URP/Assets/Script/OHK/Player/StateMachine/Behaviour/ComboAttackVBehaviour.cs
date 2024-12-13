using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȯ�� �޺������� ����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class ComboAttackVBehaviour : StateMachineBehaviour
{
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

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
	}

	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
	}
}
