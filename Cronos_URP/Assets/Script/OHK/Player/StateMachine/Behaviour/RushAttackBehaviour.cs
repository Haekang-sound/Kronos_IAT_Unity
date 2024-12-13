using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 돌진공격의 행동을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class RushAttackBehaviour : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;
	
	[SerializeField] private float _moveForce;
	[SerializeField] private float _damage;

	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerRushAttackState(_stateMachine));

		_stateMachine.MoveForce = _moveForce;
		_stateMachine.HitStop.hitStopTime = hitStopTime;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = _damage;
		Player.Instance.CurrentDamage = _damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;

	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine.currentStateInformable = stateInfo;
		_stateMachine.minf = minFrame;
	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
