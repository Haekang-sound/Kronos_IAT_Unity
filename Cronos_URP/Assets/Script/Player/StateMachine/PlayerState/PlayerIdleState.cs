using System.Runtime.InteropServices;
using UnityEngine;


// �⺻����
// �ִϸ��̼� : idle
// ��
public class PlayerIdleState : PlayerBaseState
{
	private readonly int idleHash = Animator.StringToHash("Idle");
	private readonly float duration = 0.3f;
	private bool isMove = false;

	float releaseLockOn = 0f;

	public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;

		stateMachine.InputReader.onSwitchingStart += Deceleration;

		stateMachine.InputReader.onMove += IsMove;
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateMachine.Animator.SetTrigger("Attack");
			//stateMachine.SwitchState(new PlayerAttackState(stateMachine));
		}

		// playerComponent�������� ���� ������� �ʴٸ�
// 		if (!IsGrounded())
// 		{
// 			//stateMachine.SwitchState(new PlayerFallState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
// 		}
		// �����̸� == �̵�Ű�Է��� ������
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵����·� �ٲ��
			stateMachine.Animator.SetBool("isMove", true);
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		}

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

	}
	public override void FixedTick() { }
	public override void LateTick() { }
	public override void Exit()
	{
		stateMachine.InputReader.onMove -= IsMove;
		//stateMachine.InputReader.onLAttackStart -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
		//stateMachine.InputReader.onLockOnStart -= LockOn;

		stateMachine.InputReader.onSwitchingStart -= Deceleration;
	}

	private void SwitchToLAttackState()
	{
		stateMachine.Animator.SetTrigger("Attack");
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
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

	private void LockOn()
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

	private void IsMove()
	{
		isMove = true;
	}
	private void SwitchToParryState()
	{
		Debug.Log("������");
		stateMachine.SwitchState(new PlayerParryState(stateMachine));
	}
	private void SwitchToMoveState()
	{
		stateMachine.SwitchState(new PlayerMoveState(stateMachine));
	}
}
