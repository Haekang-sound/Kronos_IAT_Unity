using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player의 
/// 시간베기 상태를 정의하는 클래스
/// (현재 사용하지 않음)
/// 
/// ohk    v1
/// </summary>
public class PlayerTimeSlashState : PlayerBaseState
{
	public PlayerTimeSlashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] private float _moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	private Vector3 _targetPosition;

	public override void Enter()
	{
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.MoveForce = _moveForce;
		_stateMachine.HitStop.hitStopTime = hitStopTime;

		_stateMachine.GroundChecker.toggleChecker = false;

		// 도착해야할 위치
		_targetPosition = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.forward * 1f;
	}

	public override void Tick()
	{
		// 타겟과 캐릭터사이의 거리가 1보다 크다면 타겟쪽으로 다가간다.
		if ((_targetPosition - _stateMachine.transform.position).magnitude > 1f)
		{
			_stateMachine.GetComponent<Collider>().isTrigger = true;

			if (Mathf.Sqrt((_targetPosition - _stateMachine.transform.position).magnitude) < 40f)
			{
				_stateMachine.Rigidbody.velocity += (_targetPosition - _stateMachine.transform.position).normalized
				* (_targetPosition - _stateMachine.transform.position).magnitude * (_targetPosition - _stateMachine.transform.position).magnitude;
			}
			else
			{
				_stateMachine.Rigidbody.velocity += (_targetPosition - _stateMachine.transform.position).normalized * 40f;
			}


		}
		else // 도착하면 멈춘다.
		{
			_stateMachine.GetComponent<Collider>().isTrigger = false;
			_stateMachine.Rigidbody.velocity = Vector3.zero;
		}
	}

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick() { }

	public override void Exit()
	{

		_stateMachine.GroundChecker.toggleChecker = true;
	}

	Transform GetTopParent(Transform current)
	{
		// 부모가 없으면 자신이 최상위
		if (current.parent == null || current.CompareTag("Respawn"))
		{
			return current;
		}
		// 부모가 있으면 부모로 재귀적으로 올라감
		return GetTopParent(current.parent);
	}


}

