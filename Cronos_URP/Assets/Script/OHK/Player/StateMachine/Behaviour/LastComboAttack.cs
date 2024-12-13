using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 콤보어택의 마지막 동작을 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class LastComboAttack : StateMachineBehaviour
{
	[SerializeField] private float _moveForce;
	[SerializeField] private float _damage;
	public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	[SerializeField] private bool _stopDodge;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		PlayerStateMachine.GetInstance().DodgeBool = _stopDodge;
		PlayerStateMachine.GetInstance().SwitchState(new PlayerAttackState(PlayerStateMachine.GetInstance()));
		PlayerStateMachine.GetInstance().MoveForce = _moveForce;
		PlayerStateMachine.GetInstance().currentLayerIndex = layerIndex;
		PlayerStateMachine.GetInstance().HitStop.hitStopTime = hitStopTime;

		Player.Instance.damageable.enabled = false;

		Player.Instance.meleeWeapon.simpleDamager.damageAmount = _damage;
		Player.Instance.CurrentDamage = _damage;
		Player.Instance.meleeWeapon.simpleDamager.currentDamageType = damageType;
		Player.Instance.meleeWeapon.simpleDamager.currentComboType = comboType;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// 이동키 입력을 확인한다.
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}
		else
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, false);
		}

	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
 		PlayerStateMachine.GetInstance().Player.damageable.enabled = true;
	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Implement code that processes and affects root motion
	}
}
