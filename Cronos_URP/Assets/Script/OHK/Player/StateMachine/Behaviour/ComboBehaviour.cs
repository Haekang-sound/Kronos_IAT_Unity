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
// 	/// 키입력
// 		if (Input.GetKeyDown(KeyCode.Mouse1))
// 		{
// 			animator.SetBool(guradHash, true);
// 		}

// 	// 이동키입력을 받으면
	if (stateMachine.InputReader.moveComposite.magnitude != 0f)
	{
		// 이동중
		animator.SetBool(moveHash, true);
	}
	else// 혹은
	{
		// 이동아님
		animator.SetBool(moveHash, false);
	}

// 	
// 	/// 좌클릭시
//  		if ((Input.GetKeyDown(KeyCode.Mouse0) && stateInfo.normalizedTime < minFrame))
//  		{
//  			attackBool = true;
//  		}
// 	if ((Input.GetKeyDown(KeyCode.Mouse0) || attackBool) && stateInfo.normalizedTime > minFrame)
// 	if (attackBool && stateInfo.normalizedTime > minFrame)
// 		{
// 			// NEXTCOMBO 활성화
// 			animator.SetBool(nextComboHash, true);
// 		}
// 
// 		// 좌클릭 누르는 중에는 차징
// 		if (stateMachine.InputReader.IsLAttackPressed)
// 		{
// 			float current = animator.GetFloat(chargeHash);
// 			animator.SetFloat(chargeHash, current + Time.deltaTime);
// 		}
// 
// 		// 누르고있으면 차징중이다
// 		if (stateMachine.InputReader.IsLAttackPressed)
// 		{
// 			//인풋중에 뭐라고 정해줘야할듯
// 			animator.SetBool(chargeAttackHash, true);
// 		}
// 		else
// 		{
// 			//인풋중에 뭐라고 정해줘야할듯
// 			animator.SetBool(chargeAttackHash, false);
// 		}
// 
// 		// 좌클릭땔때 차징 비활성화
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
		Debug.Log($"몇개냐 {handlerCount}, 이름은 {currentStateInfo.shortNameHash} ");
		/// 좌클릭시
		if (currentStateInfo.normalizedTime < minFrame)
		{
			attackBool = true;
		}
		if (/*(Input.GetKeyDown(KeyCode.Mouse0) || attackBool) && */currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO 활성화
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
