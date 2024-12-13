using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Damageable;

/// <summary>
/// ��ȭ�޺��� ������ ���ݽ� �ൿ�� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class LastEnforcedCombo : StateMachineBehaviour
{
	private PlayerStateMachine _stateMachine;

	[SerializeField] private float _moveForce;
	[SerializeField] private float _damage;
    public Damageable.DamageType damageType;
	public Damageable.ComboType comboType;

	public float hitStopTime;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_stateMachine = PlayerStateMachine.GetInstance();
		_stateMachine.SwitchState(new PlayerAttackState(_stateMachine));

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

        // �̵����θ� �Ǵ��Ѵ�.
        if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}
		else
		{
			animator.SetBool(PlayerHashSet.Instance.IsMove, false);
		}

	}

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    // Implement code that processes and affects root motion
	}
}
