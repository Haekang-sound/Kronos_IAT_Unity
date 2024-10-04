using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ComboBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");

	[SerializeField] float moveForce;
	[SerializeField] float Damage;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;

		animator.SetBool(nextComboHash, false);
		animator.ResetTrigger("Attack");
		animator.ResetTrigger("Rattack");
	}


	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine.currentStateInformable = stateInfo;
		stateMachine.minf = minFrame;

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

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
