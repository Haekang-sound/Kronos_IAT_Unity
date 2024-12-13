using System.Runtime.InteropServices;
using UnityEngine;


/// <summary>
/// Player�� ���޻��¸� �����ϴ� Ŭ����
/// (���� ������� �ʴ´�.)
/// 
/// ohk    v1
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
	float releaseLockOn = 0f;

	public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		_stateMachine.InputReader.onDecelerationStart += Deceleration;

		_stateMachine.InputReader.onMove += IsMove;
		_stateMachine.Rigidbody.velocity = Vector3.zero;
	}

	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
		}

		// �����̸� == �̵�Ű�Է��� ������
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵����·� �ٲ��
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}

		if (Input.GetMouseButtonDown(2))
		{
			// ���� ���°� �ƴ϶��
			if (!_stateMachine.Player.IsLockOn)
			{
				// ����� ã��
				_stateMachine.Player.IsLockOn = _stateMachine.autoTargetting.FindTarget();
			}
			// ���»��¶�� ������ �����Ѵ�.
			else
			{
				_stateMachine.autoTargetting.SwitchTarget();
			}
		}

		if (Input.GetMouseButton(2))
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				_stateMachine.autoTargetting.LockOff();
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
		_stateMachine.InputReader.onMove -= IsMove;
		_stateMachine.InputReader.onDecelerationStart -= Deceleration;

	}

	private void SwitchToLAttackState()
	{
		_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.SwitchState(new PlayerAttackState(_stateMachine));
	}

	private void SwitchToDefanceState()
	{
		_stateMachine.SwitchState(new PlayerGuardState(_stateMachine));
	}

	private void Deceleration()
	{
		if (_stateMachine.Player.CP >= 100)
		{
			Debug.Log("���͵��� ��������");
			BulletTime.Instance.DecelerateSpeed();
			_stateMachine.Player.IsDecreaseCP = true;
		}

	}

	private void LockOn()
	{
		// ���� ���°� �ƴ϶��
		if (!_stateMachine.Player.IsLockOn)
		{
			// ����� ã��
			_stateMachine.Player.IsLockOn = _stateMachine.autoTargetting.FindTarget();
		}
		// ���»��¶�� ������ �����Ѵ�.
		else
		{
			//stateMachine.AutoTargetting.LockOff();
			_stateMachine.autoTargetting.SwitchTarget();
		}
	}

	private void IsMove()
	{
		//isMove = true;
	}

	private void SwitchToParryState()
	{
		Debug.Log("������");
		_stateMachine.SwitchState(new PlayerDodgeState(_stateMachine));
	}

	private void SwitchToMoveState()
	{
		_stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
	}
}
