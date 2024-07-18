using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEditorInternal;

//using UnityEditorInternal;
using UnityEngine;

public class ComboBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int chargeHash = Animator.StringToHash("Charge");
	private readonly int chargeAttackHash = Animator.StringToHash("chargeAttack");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");

	[SerializeField] float moveForce;

	public float hitStopTime;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;
		animator.SetBool(nextComboHash, false);
		animator.ResetTrigger("Attack");
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

		if (stateMachine.Velocity.z != 0f)
		{
			int a = 3;
		}
		/// Ű�Է�
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			animator.SetBool(guradHash, true);
		}
		// �̵�Ű�Է��� ������
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵���
			animator.SetBool(moveHash, true);
		}
		else// Ȥ��
		{
			// �̵��ƴ�
			animator.SetBool(moveHash, false);
		}

		// ��Ŭ����
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			// NEXTCOMBO Ȱ��ȭ
			animator.SetBool(nextComboHash, true);
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
		
		// ��Ŭ������ ��¡ ��Ȱ��ȭ
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			animator.SetFloat(chargeHash, 0);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger(dodgeHash);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
// 	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
// 	{
// 
// 	}
	// OnStateMove is called right after Animator.OnAnimatorMove()
// 	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
// 	{
// 		// Implement code that processes and affects root motion
// 
// 	}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
