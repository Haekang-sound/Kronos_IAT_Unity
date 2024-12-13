using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player의 공격상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerAttackState : PlayerBaseState
{
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] private float _moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	
	private bool _attackBool = false;
	private Vector3 _targetPosition;
	private AnimatorStateInfo _currentStateInfo;

	public override void Enter()
	{
		_attackBool = false;
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.MoveForce = _moveForce;
		_stateMachine.HitStop.hitStopTime = hitStopTime;

		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);

		_stateMachine.InputReader.onLAttackStart += Attack;
		_stateMachine.InputReader.onRAttackStart += Gurad;
		_stateMachine.InputReader.onJumpStart += Dodge;

		_stateMachine.GroundChecker.toggleChecker = false;
	}

	public override void Tick()
	{
		// 자동조준 여부에따라
		// 바라볼 방향을 결정한다.
		if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			_targetPosition = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.forward ;
		}
		else
		{
			CalculateMoveDirection();
		}

		// 조건을 확인하고
		// 다음 콤보공격으로 넘어가는 조건을 활성화한다.
		if (_attackBool && _stateMachine.currentStateInformable.normalizedTime > _stateMachine.minf)
		{
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}

		// 좌클릭 누르는 중에는 차징
		if (_stateMachine.InputReader.IsLAttackPressed)
		{
			float current = _stateMachine.Animator.GetFloat(PlayerHashSet.Instance.Charge);
			_stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, current + Time.deltaTime);
		}

		// 좌클릭을 누르고있으면 차징중이다
		if (_stateMachine.InputReader.IsLAttackPressed)
		{
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.ChargeAttack, true);
		}
		else
		{
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.ChargeAttack, false);
		}

		// 좌클릭을 멈췄을때 차징정보를 초기화
		if (!_stateMachine.InputReader.IsLAttackPressed)
		{
			_stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		}

		CalculateMoveDirection(); 

		Vector3 gravity = Vector3.down * Mathf.Abs(_stateMachine.Rigidbody.velocity.y);

		// 자동조준이 활성화 되었을때
		// 플레이어와 몬스터의 거리에 따라서
		// 몬스터방향으로 접근여부를 결정하는 부분
		if (Time.deltaTime == 0f) return;
		else if (_stateMachine.autoTargetting.GetTarget() != null)
		{
			if ((_targetPosition - _stateMachine.transform.position).magnitude 
				> _stateMachine.Player.targetDistance)
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
		// 애니메이션 트랜지션 중일때만 
		// 캐릭터의 진행방향을 바꿀 수 있게한다
		if (_stateMachine.Animator.IsInTransition(_stateMachine.currentLayerIndex))
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
		_stateMachine.InputReader.onLAttackStart -= Attack;
		_stateMachine.InputReader.onRAttackStart -= Gurad;
		_stateMachine.InputReader.onJumpStart -= Dodge;

		_stateMachine.GroundChecker.toggleChecker = true;
	}

	private void Attack()
	{
		/// 좌클릭시
		_stateMachine.autoTargetting.AutoTargeting();
		_attackBool = true;

		if (_attackBool && _currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO 활성화
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}
	}

	private void Dodge()
	{
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f && !CoolTimeCounter.Instance.IsDodgeUsed)
		{
			CoolTimeCounter.Instance.IsDodgeUsed = true;
			if (_stateMachine.InputReader.moveComposite.magnitude != 0)
			{
				_stateMachine.Rigidbody.rotation = Quaternion.LookRotation(_stateMachine.velocity);
			}
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);    // 
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
		}
	}

	private void Gurad()
	{
		if (_stateMachine.IsRattack)
		{
			_stateMachine.IsRattack = false;
			return;
		}
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, true);
	}

}
