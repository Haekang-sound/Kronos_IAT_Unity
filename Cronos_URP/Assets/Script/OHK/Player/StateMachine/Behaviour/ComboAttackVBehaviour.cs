using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 변환된 콤보어택을 사용하는 클래스
/// 
/// ohk    v1
/// </summary>
public class ComboAttackVBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		/// R어택이 가능한 상태라는 것을 알린다.
		bool isVariation = animator.GetBool("ComAttackVariation");

		// 활성화 되었다면
		if (isVariation)
		{
			// 우클릭을 rattack으로 바꿔라
			PlayerStateMachine.GetInstance().InputReader.onRAttackStart += RAttack;
			PlayerStateMachine.GetInstance().IsRattack = true;
		}
	}
	public void RAttack()
	{
		Debug.Log("다른공격을 하겠어요");
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
