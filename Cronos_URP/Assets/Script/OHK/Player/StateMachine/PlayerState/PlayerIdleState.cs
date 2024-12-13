using System.Runtime.InteropServices;
using UnityEngine;


/// <summary>
/// Player의 유휴상태를 정의하는 클래스
/// (현재 사용하지 않는다.)
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

		// 움직이면 == 이동키입력을 받으면
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// 이동상태로 바뀐다
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsMove, true);
		}

		if (Input.GetMouseButtonDown(2))
		{
			// 락온 상태가 아니라면
			if (!_stateMachine.Player.IsLockOn)
			{
				// 대상을 찾고
				_stateMachine.Player.IsLockOn = _stateMachine.autoTargetting.FindTarget();
			}
			// 락온상태라면 락온을 해제한다.
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
			Debug.Log("몬스터들이 느려진다");
			BulletTime.Instance.DecelerateSpeed();
			_stateMachine.Player.IsDecreaseCP = true;
		}

	}

	private void LockOn()
	{
		// 락온 상태가 아니라면
		if (!_stateMachine.Player.IsLockOn)
		{
			// 대상을 찾고
			_stateMachine.Player.IsLockOn = _stateMachine.autoTargetting.FindTarget();
		}
		// 락온상태라면 락온을 해제한다.
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
		Debug.Log("구른다");
		_stateMachine.SwitchState(new PlayerDodgeState(_stateMachine));
	}

	private void SwitchToMoveState()
	{
		_stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
	}
}
