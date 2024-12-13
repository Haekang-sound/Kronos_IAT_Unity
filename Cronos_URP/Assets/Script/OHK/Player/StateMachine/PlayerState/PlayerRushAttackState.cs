using TMPro;
using UnityEngine;

/// <summary>
/// Player의 
/// 돌진공격상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerRushAttackState : PlayerBaseState
{
	public PlayerRushAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] private float _moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	private Vector3 _targetPosition;

	public override void Enter()
	{
		_stateMachine.autoTargetting.GetComponent<SphereCollider>().radius = 10f;
		_stateMachine.autoTargetting.AutoTargeting();
		
		_stateMachine.MoveForce = _moveForce;
		_stateMachine.HitStop.hitStopTime = hitStopTime;

		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);

		_stateMachine.GroundChecker.toggleChecker = false;

	}

	public override void Tick()
	{
		_stateMachine.autoTargetting.AutoTargeting();

		// 자동조준 여부에따라
		// 바라볼 방향을 결정한다.
		if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			_targetPosition = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.forward * 1f;
		}
		else
		{
			CalculateMoveDirection();
		}

		Vector3 gravity = Vector3.down * Mathf.Abs(_stateMachine.Rigidbody.velocity.y);

		// 자동조준이 활성화 되었을때
		// 플레이어와 몬스터의 거리에 따라서
		// 몬스터방향으로 접근여부를 결정하는 부분
		if (Time.deltaTime == 0f) return;
		else if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			Debug.Log((_targetPosition - _stateMachine.transform.position).magnitude);
			// 타겟과 캐릭터사이의 거리가 1보다 크다면 타겟쪽으로 다가간다.
			if ((_targetPosition - _stateMachine.transform.position).magnitude 
				> 1f)
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
			else // 도착하면 멈춘다.
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
			if (_stateMachine.autoTargetting.GetTarget() != null)
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
		_stateMachine.GroundChecker.toggleChecker = true;
		_stateMachine.autoTargetting.GetComponent<SphereCollider>().radius = 5f;
	}


}
