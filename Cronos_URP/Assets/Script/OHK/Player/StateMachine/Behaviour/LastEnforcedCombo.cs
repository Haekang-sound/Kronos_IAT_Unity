using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damageable;

public class LastEnforcedCombo : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;
	[SerializeField] float moveForce;
	[SerializeField] float Damage;
    public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	public float hitStopTime;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        stateMachine.currentStateInformable = stateInfo;
        // 		if (Input.GetKeyDown(KeyCode.Mouse1))
        // 		{
        // 			animator.SetBool(guradHash, true);
        // 		}
        // 이동키입력을 받으면
        if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(PlayerHashSet.Instance.isMove, true);
		}
		else// 혹은
		{
			// 이동아님
			animator.SetBool(PlayerHashSet.Instance.isMove, false);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
// 		Player.Instance.isBuff = false;
// 		Player.Instance.IsEnforced = false;
// 		EffectManager.Instance.SwordAuraOff();
// 		Player.Instance.buffTimer = 0f;

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
