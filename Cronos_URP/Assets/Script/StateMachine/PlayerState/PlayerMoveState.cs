using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

// �÷��̾� �⺻���¸� ��ӹ��� movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
	private const float AnimationDampTime = 0.1f;
	private const float CrossFadeDuration = 0.3f;

	float releaseLockOn = 0f;


	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		//stateMachine.Animator.Rebind();
		//stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
		stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToRAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		//stateMachine.InputReader.onLockOnStart += LockOn;

		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state�� update�� �� �� ����
	public override void Tick()
	{
		// �̵����� ������ idle
		if (stateMachine.InputReader.moveComposite.magnitude == 0)
		{
			stateMachine.SwitchState(new PlayerIdleState(stateMachine));
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
		}

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent�������� ���� ������� �ʴٸ�
		if (!IsGrounded())
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
		}

		float moveSpeed = 0.5f;

		if (Input.GetButton("Run"))
		{
			moveSpeed *= 2;
		}
		else { moveSpeed = 0.5f; }

		stateMachine.Player.SetSpeed(moveSpeed);

		if (Input.GetMouseButtonDown(2))
		{
			// ���� ���°� �ƴ϶��
			if (!stateMachine.Player.IsLockOn)
			{
				// ����� ã��
				stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			}
			// ���»��¶�� ������ �����Ѵ�.
			else
			{
				//stateMachine.AutoTargetting.LockOff();
				stateMachine.AutoTargetting.SwitchTarget();
			}
		}

		if (Input.GetMouseButton(2))
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				stateMachine.AutoTargetting.LockOff();
			}
		}
		else
		{
			releaseLockOn = 0f;
		}

		// �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);

		CalculateMoveDirection();   // ������ ����ϰ�

	}
	public override void FixedTick()
	{
		FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
		Move();                     // �̵��Ѵ�.	
	}

	public override void LateTick() { }

	public override void Exit()
	{
		// ���¸� Ż���Ҷ��� jump�� ���� Action�� �������ش�.
		stateMachine.InputReader.onJumpPerformed -= SwitchToParryState;
		stateMachine.InputReader.onLAttackStart -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToRAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;

		stateMachine.InputReader.onSwitchingStart -= Deceleration;

	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 100)
		{
			Debug.Log("���͵��� ��������");
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	private void SwitchToParryState()
	{
		Debug.Log("������");
		stateMachine.SwitchState(new PlayerParryState(stateMachine));
	}

	private void SwitchToLAttackState()
	{
		//stateMachine.Animator.SetTrigger("Attack");
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}

	private void SwitchToRAttackState()
	{
		stateMachine.Animator.SetBool("isGuard", true);
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
	private void LockOn()
	{
		if (!stateMachine.Player.IsLockOn)
		{
			// 			// �ڵ������� �����ϰ� 
			// 			stateMachine.AutoTargetting.LockOff();
			// ����� ã��
			stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			// lockOn�Ѵ�.
			//stateMachine.AutoTargetting.LockOn();
		}
		else
		{
			stateMachine.AutoTargetting.LockOff();
			//stateMachine.AutoTargetting.SwitchTarget();
		}

	}
}




