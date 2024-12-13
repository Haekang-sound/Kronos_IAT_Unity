using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static Damageable;

/// <summary>
/// 강화공격을 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class ComboAttackBehaviour : StateMachineBehaviour
{
	PlayerStateMachine stateMachine;

	[SerializeField] private float _moveForce;
	[SerializeField] private float _damage;

	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	[SerializeField] private bool _stopDodge;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine = PlayerStateMachine.GetInstance();
		stateMachine.DodgeBool = _stopDodge;
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));

		stateMachine.MoveForce = _moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = _damage;
		Player.Instance.CurrentDamage = _damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;

		animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		stateMachine.currentStateInformable = stateInfo;
		stateMachine.minf = minFrame;

		// 	// 이동키입력을 받으면
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// 이동중
			animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}
		else// 혹은
		{
			// 이동아님
			animator.SetBool(PlayerHashSet.Instance.IsMove, false);
		}

	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
