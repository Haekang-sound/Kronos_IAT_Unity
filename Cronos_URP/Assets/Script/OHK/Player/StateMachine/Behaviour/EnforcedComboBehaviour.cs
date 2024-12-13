using UnityEngine;

/// <summary>
/// 강화콤보공격시 동작을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class EnforcedComboBehaviour : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

		if (Input.GetKey(KeyCode.Mouse0))
		{
			float current = animator.GetFloat(PlayerHashSet.Instance.Charge);
			animator.SetFloat(PlayerHashSet.Instance.Charge, current + Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		}

		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			// nextcombo 가 예정되어있다면
			if (animator.GetBool(PlayerHashSet.Instance.NextCombo))
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

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}
}
