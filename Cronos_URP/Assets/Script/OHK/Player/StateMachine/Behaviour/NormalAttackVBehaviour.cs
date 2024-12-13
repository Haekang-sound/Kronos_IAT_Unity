using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 변환된 노멀어택의
/// 행도을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class NormalAttackVBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// 우클릭 공격이 가능한 상태라는 것을 알린다.
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
