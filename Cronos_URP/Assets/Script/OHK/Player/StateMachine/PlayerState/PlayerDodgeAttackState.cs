using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player의 
/// 회피 공격상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerDodgeAttackState : PlayerBaseState
{
	public PlayerDodgeAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	[SerializeField] private float _moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	private Vector3 _targetPosition;

	public override void Enter()
	{
		_stateMachine.autoTargetting.AutoTargeting();
		_stateMachine.MoveForce = _moveForce;
		_stateMachine.HitStop.hitStopTime = hitStopTime;
		
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);

		_stateMachine.GroundChecker.toggleChecker = false;
	}

	public override void Tick()
	{
		// 자동조준 여부에따라
		// 바라볼 방향을 결정한다.
		if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			_targetPosition = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position;
		}
		else
		{
			CalculateMoveDirection(); 
		}

		CalculateMoveDirection();   // 방향을 계산하고

		Vector3 gravity = Vector3.down * Mathf.Abs(_stateMachine.Rigidbody.velocity.y);

		// 자동조준이 활성화 되었을때
		// 플레이어와 몬스터의 거리에 따라서
		// 몬스터방향으로 접근여부를 결정하는 부분
		if (Time.deltaTime == 0f) return;
		else if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			if ((_targetPosition - _stateMachine.transform.position).magnitude > 1f)
			{
				if (_stateMachine.MoveForce > 1f && _stateMachine.Animator.deltaPosition != null)
				{
					_stateMachine.Rigidbody.velocity = (_stateMachine.Animator.deltaPosition / Time.deltaTime) * _stateMachine.MoveForce + gravity;
				}
				else if (_stateMachine.Animator.deltaPosition != null)
				{
					_stateMachine.Rigidbody.velocity = (_stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
				}

			}
			else 
			{
				_stateMachine.Rigidbody.velocity = Vector3.zero;
			}

		}
		else
		{
			if (_stateMachine.MoveForce > 1f && _stateMachine.Animator.deltaPosition != null)
			{
				_stateMachine.Rigidbody.velocity = (_stateMachine.Animator.deltaPosition / Time.deltaTime) * _stateMachine.MoveForce + gravity;
			}
			else if (_stateMachine.Animator.deltaPosition != null)
			{
				_stateMachine.Rigidbody.velocity = (_stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
			}
		}
	}

	public override void FixedTick()
	{
		///트랜지션 중일때만 발동
		//if (stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex))
		{
			if (_stateMachine.autoTargetting.GetTarget() != null && _stateMachine.InputReader.moveComposite.magnitude == 0f)
			{
				FaceMoveDirection((_targetPosition - _stateMachine.transform.position).normalized);
			}
			else
			{
				FaceMoveDirection();
			}
		}
		Float();
	}

	public override void LateTick() { }

	public override void Exit()
	{
		_stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.ChargeAttack, false);
		_stateMachine.GroundChecker.toggleChecker = true;
	}

}
