using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;


//using UnityEditorInternal;

//using UnityEditorInternal;

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
	bool attackBool = false;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		currentStateInfo = stateInfo;

		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		attackBool = false;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		animator.SetBool(nextComboHash, false);
		animator.ResetTrigger("Attack");
// 
//         InputReader.GetInstance().onLAttackStart += Attack;
//         InputReader.GetInstance().onRAttackStart += Gurad;
//         InputReader.GetInstance().onJumpStart += Dodge;
	}


	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine.currentStateInformable = stateInfo;
		stateMachine.minf = minFrame;
		

// 		if (stateMachine.Velocity.z != 0f)
// 		{
// 			//int a = 3;
// 		}
// 	/// Ű�Է�
// 		if (Input.GetKeyDown(KeyCode.Mouse1))
// 		{
// 			animator.SetBool(guradHash, true);
// 		}

// 	// �̵�Ű�Է��� ������
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

// 	
// 	/// ��Ŭ����
//  		if ((Input.GetKeyDown(KeyCode.Mouse0) && stateInfo.normalizedTime < minFrame))
//  		{
//  			attackBool = true;
//  		}
// 	if ((Input.GetKeyDown(KeyCode.Mouse0) || attackBool) && stateInfo.normalizedTime > minFrame)
// 	if (attackBool && stateInfo.normalizedTime > minFrame)
// 		{
// 			// NEXTCOMBO Ȱ��ȭ
// 			animator.SetBool(nextComboHash, true);
// 		}
// 
// 		// ��Ŭ�� ������ �߿��� ��¡
// 		if (stateMachine.InputReader.IsLAttackPressed)
// 		{
// 			float current = animator.GetFloat(chargeHash);
// 			animator.SetFloat(chargeHash, current + Time.deltaTime);
// 		}
// 
// 		// ������������ ��¡���̴�
// 		if (stateMachine.InputReader.IsLAttackPressed)
// 		{
// 			//��ǲ�߿� ����� ��������ҵ�
// 			animator.SetBool(chargeAttackHash, true);
// 		}
// 		else
// 		{
// 			//��ǲ�߿� ����� ��������ҵ�
// 			animator.SetBool(chargeAttackHash, false);
// 		}
// 
// 		// ��Ŭ������ ��¡ ��Ȱ��ȭ
// 		if (!stateMachine.InputReader.IsLAttackPressed)
// 		{
// 			animator.SetFloat(chargeHash, 0);
// 		}

// 		if (Input.GetKeyDown(KeyCode.Space))
// 		{
// // 			if (stateMachine.Velocity.magnitude != 0f)
// // 			{
// // 				stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
// // 				animator.SetTrigger(dodgeHash);
// // 			}
// 		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
// 		animator.SetFloat(chargeHash, 0);
// 		animator.SetBool(chargeAttackHash, false);
// 		stateMachine.InputReader.onLAttackStart -= Attack;
// 		stateMachine.InputReader.onRAttackStart -= Gurad;
// 		stateMachine.InputReader.onJumpStart -= Dodge;

		

	}
	private void Attack()
	{
		int handlerCount = stateMachine.InputReader.onLAttackStart?.GetInvocationList().Length ?? 0;
		Debug.Log($"��� {handlerCount}, �̸��� {currentStateInfo.shortNameHash} ");
		/// ��Ŭ����
		if (currentStateInfo.normalizedTime < minFrame)
		{
			attackBool = true;
		}
		if (/*(Input.GetKeyDown(KeyCode.Mouse0) || attackBool) && */currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO Ȱ��ȭ
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
	}
	private void Dodge()
	{
		if (stateMachine.Velocity.magnitude != 0f)
		{
			stateMachine.Animator.SetBool(nextComboHash, false);
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
			stateMachine.Animator.SetTrigger(dodgeHash);
		}
	}
	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(guradHash, true); }

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//     override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//     {
	// 
	//     }

}
