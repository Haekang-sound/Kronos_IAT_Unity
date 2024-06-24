using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerAttackState : PlayerBaseState
{
	AnimatorStateInfo animatorStateInfo;
	private readonly int AttackHash1 = Animator.StringToHash("Combo_01_1");
	private readonly int AttackHash2 = Animator.StringToHash("Combo_01_2");
	private readonly int AttackHash3 = Animator.StringToHash("Combo_01_3");
	private readonly int AttackHash4 = Animator.StringToHash("Combo_02_3");
	private readonly int AttackHash5 = Animator.StringToHash("Combo_03_3");
	private readonly int AttackHash6 = Animator.StringToHash("Combo_03_4");

	private readonly int chargeAtdtackHash = Animator.StringToHash("Combo_03_4");   // ��ȭ����

	float normalizedTime = 0f;

	private const float CrossFadeDuration = 0.1f;

	private float chargeAttack = 0f;

	public float startNormalizedTime = 0.3f;    // ���� ����
	public float endNormalizedTime = 0.99f;     // ���� ����

	List<int> comboAttack;

	int comboStack = 0;

	private bool nextCombo = false;
	private bool isEnforcedAttack = false;      // ��ȭ���� ����
	private bool isEnforcedAttackDone = false;  // ��ȭ������ ����


	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		// �޺� �ؽ�����Ʈ
		comboAttack = new List<int>();
		// �ִϸ��̼� �ؽ����� �߰��Ѵ�.
		comboAttack.Add(AttackHash1);
		comboAttack.Add(AttackHash2);
		comboAttack.Add(AttackHash3);
		comboAttack.Add(AttackHash4);
		comboAttack.Add(AttackHash5);
		comboAttack.Add(AttackHash6);

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

		// ���콺�� ����������
		if(stateMachine.InputReader.IsLAttackPressed)
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
		// ���������� 70% �̻��̶��
		else if (nextCombo && normalizedTime > 0.7f)
		{
				// ���ο� �޺������� �����Ѵ�.
				NextCombo();
		}


		if (normalizedTime >= 1.0f)
		{
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine));
			stateMachine.SwitchState(new PlayerBuffState(stateMachine));
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
		
		//stateMachine.Player.IsEnforced = true;
	}

	// �����޺��� �غ��Ѵ�.
	public void ReadyNextCombo()
	{
		// ���� ������ �����Ǿ��ִٸ� �����Ѵ�.
		if (nextCombo == true || comboStack == 5)
		{
			return;
		}

		// ����.. ���
		// 
		if(normalizedTime > 0.3f && normalizedTime < 0.7f)
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
		stateMachine.Animator.CrossFade(comboAttack[comboStack], 0.1f, -1, 0f);
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
