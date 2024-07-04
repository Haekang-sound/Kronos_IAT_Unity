using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerEnforcedAttackState : PlayerBaseState
{
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int AttackHash1 = Animator.StringToHash("Com_Attack_01");
	private readonly int chargeAtdtackHash = Animator.StringToHash("Com_S Attack");   // ��ȭ����

	private const float CrossFadeDuration = 0.1f;

	private float chargeAttack = 0f;

	public float startNormalizedTime = 0.3f;    // ���� ����
	public float endNormalizedTime = 0.99f;     // ���� ����

	private bool isEnforcedAttack = false;      // ��ȭ���� ����
	private bool isEnforcedAttackDone = false;  // ��ȭ������ ����


	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(AttackHash1, CrossFadeDuration);

// 		stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
// 		stateMachine.InputReader.onLAttackPerformed += ChargeAttack;
// 		stateMachine.InputReader.onLAttackCanceled += ResetCharge;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		/// 2024.7.4
		/// ��ǲ�ý����� ���峵���� ��ǲ�Ŵ����� ����Ѵ�... �ФФ� 
		/// �ָ����� ��ĥ �� �ְ���? ��ĥ �� �ִٰ� ���� �ذ���
		/// 

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			stateMachine.Player.ChargeAttack = 0f;
		}
		//		if (stateMachine.InputReader.IsLAttackPressed) 
		if (Input.GetKey(KeyCode.Mouse0))
		{
			// ��¡�Ѵ�.
			stateMachine.Player.ChargeAttack += Time.deltaTime;

			if (stateMachine.Player.ChargeAttack >= 0.3f)
			{
				// ��ȭ������ true�� ���ش�
				isEnforcedAttack = true;
			}
		}

		// ��ȭ������ ���డ���ϴٸ� (��ȭ������ ������ �ʾҴٸ�)
		if (isEnforcedAttack && !isEnforcedAttackDone)
		{
			// ��ȭ������ �����Ѵ�.
			EnforcedAttack();
		}

	}

	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		//stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
		//stateMachine.InputReader.onLAttackPerformed -= ChargeAttack;
		//stateMachine.InputReader.onLAttackCanceled -= ResetCharge;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

	}

	// �����޺��� �غ��Ѵ�.
	public void ReadyNextCombo()
	{
		// ���� �޺��� �غ��Ѵ�.
		stateMachine.Animator.SetBool(nextComboHash, true);
	}

	// ��ȭ������ �����Ѵ�.
	public void EnforcedAttack()
	{
		//stateMachine.Animator.Rebind();
		//float normalizedStartTime = 0.0f;
		// �ִϸ��̼��� �����ϰ�
		stateMachine.Animator.CrossFadeInFixedTime(chargeAtdtackHash, 0.1f, -1, 0.3F);
		// ��¡�� �����Ѵ�.
		ResetCharge();
		// ��ȭ������ ������.
		isEnforcedAttackDone = true;
	}

	public void ChargeAttack()
	{
		// ��¡�ð��� ���Ѵ�.
		chargeAttack += Time.deltaTime;

		// ���콺 �޹�ư ������ true�� �Ѵ�
		stateMachine.InputReader.IsLAttackPressed = true;
	}

	// ��¡�� �����Ѵ�.
	public void ResetCharge()
	{
		// ���콺 Ŭ���� false�� �Ѵ�.
		stateMachine.InputReader.IsLAttackPressed = false;
		// ��¡�� �����Ѵ�.
		chargeAttack = 0f;
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}
