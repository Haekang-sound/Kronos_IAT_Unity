using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ��ȯ�� ��־�����
/// �൵�� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class NormalAttackVBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// ��Ŭ�� ������ ������ ���¶�� ���� �˸���.
		bool isVariation = animator.GetBool(PlayerHashSet.Instance.NorAttackVariation);

		if (isVariation) 
		{
			PlayerStateMachine.GetInstance().InputReader.onRAttackStart += RAttack;
			PlayerStateMachine.GetInstance().IsRattack = true;
		}
	}

	public void RAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetTrigger(PlayerHashSet.Instance.Rattack);
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
		PlayerStateMachine.GetInstance().Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
	}

	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		PlayerStateMachine.GetInstance().InputReader.onRAttackStart -= RAttack;
	}
}
