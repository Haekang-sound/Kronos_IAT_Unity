using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCombo : StateMachineBehaviour
{
	[SerializeField] float moveForce;
	[SerializeField] float Damage;
	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	[SerializeField] private bool stopDodge;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		PlayerStateMachine.GetInstance().DodgeBool = stopDodge;
		PlayerStateMachine.GetInstance().SwitchState(new PlayerAttackState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().MoveForce = moveForce;
		PlayerStateMachine.GetInstance().currentLayerIndex = layerIndex;
		PlayerStateMachine.GetInstance().HitStop.hitStopTime = hitStopTime;

		//Player.Instance.IsEnforced = true;
		Player.Instance._damageable.enabled = false;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// 이동키입력을 받으면
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(PlayerHashSet.Instance.isMove, true);
		}
		else
		{
			animator.SetBool(PlayerHashSet.Instance.isMove, false);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
 		PlayerStateMachine.GetInstance().Player._damageable.enabled = true;
// 		Player.Instance.isBuff = false;
// 		Player.Instance.buffTimer = 0f;
// 		Player.Instance.IsEnforced = false;
// 		EffectManager.Instance.SwordAuraOff();
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
