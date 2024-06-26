using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerEnforcedAttackState : PlayerBaseState
{
	AnimatorStateInfo animatorStateInfo;
	private readonly int AttackHash1 = Animator.StringToHash("Attack_3Combo_1");
	private readonly int AttackHash2 = Animator.StringToHash("Attack_3Combo_2");
	private readonly int AttackHash3 = Animator.StringToHash("Attack_3Combo_3");
	private readonly int AttackHash4 = Animator.StringToHash("Attack_4Combo_1B");

	private readonly int chargeAtdtackHash = Animator.StringToHash("Combo_03_4");   // ��ȭ����

	float normalizedTime = 0f;

	private const float CrossFadeDuration = 0.1f;

	private float chargeAttack = 0f;

	public float startNormalizedTime = 0.3f;    // ���� ����
	public float endNormalizedTime = 0.99f;     // ���� ����

	List<int> comboAttack;

	private int comboStack = 0;
	private float exitTime = 0.3f;
	private float duration = 0f;
	private float offset = 0f;

	private bool nextCombo = false;
	private bool isEnforcedAttack = false;      // ��ȭ���� ����
	private bool isEnforcedAttackDone = false;  // ��ȭ������ ����


	public PlayerEnforcedAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		// �޺� �ؽ�����Ʈ
		comboAttack = new List<int>();
		// �ִϸ��̼� �ؽ����� �߰��Ѵ�.
		comboAttack.Add(AttackHash1);
		comboAttack.Add(AttackHash2);
		comboAttack.Add(AttackHash3);
		comboAttack.Add(AttackHash4);

		stateMachine.Animator.Rebind();
		stateMachine.Animator.CrossFadeInFixedTime(comboAttack[comboStack], CrossFadeDuration);
		// ���� �ִϸ��̼� ������ �޾ƿ´�

		stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
		stateMachine.InputReader.onLAttackPerformed += ChargeAttack;
		stateMachine.InputReader.onLAttackCanceled += ResetCharge;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		// ���������� ������
		animatorStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
		normalizedTime = animatorStateInfo.normalizedTime;

		switch (comboStack)
		{
			case 0:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
			case 1:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0.75f;
				duration = 0.25f;
				offset = 0f;
				break;
			case 2:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0.75f;
				duration = 0.25f;
				offset = 0f;
				break;
			case 3:
				exitTime = 0.5f;
				duration = 0.1f;
				offset = 0.05f;
				break;
				exitTime = 0f;
				duration = 31f/33f;
				offset = 0f;
				break;
		}

		// ���콺�� ����������
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			// ��¡�Ѵ�.
			chargeAttack += Time.deltaTime;

			if (chargeAttack > 0.3f)
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
			// ��ȭ������ �ߴٸ� ���� �޺������� ���� �ʴ´�.
			nextCombo = false;
		}
		// �޺��� �����Ǿ��ְ�
		// ���������� ~~ �̻��̶��
		else if (nextCombo && normalizedTime > exitTime + duration)
		{
			// ���ο� �޺������� �����Ѵ�.
			NextCombo();
		}


		if (normalizedTime >= 1.0f)
		{
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine));
			stateMachine.SwitchState(new PlayerMoveState(stateMachine));
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
		stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
		stateMachine.InputReader.onLAttackPerformed -= ChargeAttack;
		stateMachine.InputReader.onLAttackCanceled -= ResetCharge;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

	}

	// �����޺��� �غ��Ѵ�.
	public void ReadyNextCombo()
	{
		// ���� ������ �����Ǿ��ִٸ� �����Ѵ�.
		if (nextCombo == true || comboStack == 3)
		{
			return;
		}

		// ����.. ���
		// 
		if (normalizedTime > exitTime && normalizedTime < 0.7f)
		{
			// ���� �޺������� true�� �Ѵ�. 
			nextCombo = true;
		}


	}
	// ��ȭ������ �����Ѵ�.
	public void EnforcedAttack()
	{
		stateMachine.Animator.Rebind();
		float normalizedStartTime = 0.0f;
		// �ִϸ��̼��� �����ϰ�
		stateMachine.Animator.CrossFadeInFixedTime(chargeAtdtackHash, 0.1f, -1, normalizedStartTime);
		// ��¡�� �����Ѵ�.
		ResetCharge();
		// ��ȭ������ ������.
		isEnforcedAttackDone = true;
	}

	/// ���ο� �޺� �ִϸ��̼��� �����Ѵ�.
	public void NextCombo()
	{
		// �޺������� ����Ǿ��ִ� ������� �۴ٸ�
		if (comboStack < comboAttack.Count - 1)
		{
			// �޺� ������ �ø���.
			comboStack++;
		}

		// �޺����ÿ� �´� �޺� �ִϸ��̼��� �����Ѵ�.
		stateMachine.Animator.CrossFade(comboAttack[comboStack], offset, -1, 0f);
		// Ÿ���� ������
		if (stateMachine.AutoTargetting.Target != null)
		{
			// Ÿ�� ������ �̵��ϸ鼭 ����
			stateMachine.Rigidbody.AddForce((stateMachine.AutoTargetting.Target.position - stateMachine.transform.position).normalized * 2f);
		}
		// �ִϸ��̼��� �����ߴٸ� �޺� ������ �����.
		nextCombo = false;

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
