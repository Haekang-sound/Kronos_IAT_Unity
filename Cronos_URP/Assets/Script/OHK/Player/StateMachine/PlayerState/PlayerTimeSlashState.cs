using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player�� 
/// �ð����� ���¸� �����ϴ� Ŭ����
/// (���� ������� ����)
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

		// �����ؾ��� ��ġ
		_targetPosition = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.forward * 1f;
	}

	public override void Tick()
	{
		// Ÿ�ٰ� ĳ���ͻ����� �Ÿ��� 1���� ũ�ٸ� Ÿ�������� �ٰ�����.
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
		else // �����ϸ� �����.
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
		// �θ� ������ �ڽ��� �ֻ���
		if (current.parent == null || current.CompareTag("Respawn"))
		{
			return current;
		}
		// �θ� ������ �θ�� ��������� �ö�
		return GetTopParent(current.parent);
	}


}

