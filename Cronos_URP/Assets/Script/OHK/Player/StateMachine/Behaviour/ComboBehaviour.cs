using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static Damageable;

public class ComboBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;

	[SerializeField] float moveForce;
	[SerializeField] float Damage;

	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	[SerializeField] private bool stopDodge;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.DodgeBool = stopDodge;
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = Damage;
		Player.Instance.CurrentDamage = Damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;

		animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
		// 		animator.ResetTrigger("Attack");
		// 		animator.ResetTrigger("Rattack");
	}


	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine.currentStateInformable = stateInfo;
		stateMachine.minf = minFrame;

		// 	// �̵�Ű�Է��� ������
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵���
			animator.SetBool(PlayerHashSet.Instance.isMove, true);
		}
		else// Ȥ��
		{
			// �̵��ƴ�
			animator.SetBool(PlayerHashSet.Instance.isMove, false);
		}

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerStateMachine.GetInstance().Player.IsEnforced = false;
		Player.Instance.isBuff = false;
		Player.Instance.buffTimer = 0f;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
