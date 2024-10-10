using System.Collections;
using System.Collections.Generic;

//using UnityEditorInternal;
using UnityEngine;

public class LastCombo : StateMachineBehaviour
{
	private readonly int moveHash = Animator.StringToHash("isMove");
    [SerializeField] float moveForce;
    [SerializeField] float Damage;
    public Damageable.DamageType damageType;

    public float hitStopTime;
    [Range(0.0f, 1.0f)] public float minFrame;
    AnimatorStateInfo currentStateInfo;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Attack");
		PlayerStateMachine.GetInstance().HitStop.hitStopTime = hitStopTime;
        PlayerStateMachine.GetInstance().MoveForce = moveForce;
        PlayerStateMachine.GetInstance().Player.IsEnforced = true;
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = true;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;
        Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{ 
		// 이동키입력을 받으면
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(moveHash, true);
		}
		else
		{
			animator.SetBool(moveHash, false);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player.IsEnforced = false;
		PlayerStateMachine.GetInstance().Player._damageable.isInvulnerable = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    // Implement code that processes and affects root motion
	}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
