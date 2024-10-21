using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TimeSlashBehaviour : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	PlayerStateMachine stateMachine;

	[SerializeField] float moveForce;
	[SerializeField] float Damage;

	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.SwitchState(new PlayerTimeSlashState(stateMachine));

		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		BulletTime.Instance.SetNormalSpeed();
		Player.Instance.IsLockOn = false;
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
