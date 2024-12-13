using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player�� 
/// �ð����� �غ���¸� �����ϴ� Ŭ����
/// (���� ������� ����)
/// 
/// ohk    v1
/// </summary>
public class PlayerTimeSlashReadyState : PlayerBaseState
{
	public PlayerTimeSlashReadyState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		// 0) ĳ���Ϳ� ���� ���̿� ��ֹ��� ������ �����.
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.TimeSlash);
		// 1) ����,

		// 2) �ð�����
		BulletTime.Instance.DecelerateSpeed();

		// 3) ����Ÿ���� ����
		Player.Instance.IsLockOn = true;
	}

	public override void Tick()
	{
		// ī�޶��� ���� �������� ���̸� �߻�
		Vector3 pos = _stateMachine.transform.position;
		Ray ray = new Ray(pos, _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - pos);
		RaycastHit hit;
		Vector3 temp = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.position;

		// ���̸� �߻��ϰ� Ư�� ���̾ ����
		if (Physics.Raycast(ray, out hit, temp.magnitude, Player.Instance.targetLayer))
		{
			// Ư�� ���̾ ���� ������Ʈ�� ������ ���
			Player.Instance.IsLockOn = false;
			
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.GoIdle);
		}

		if (!_stateMachine.autoTargetting.isTargetting)
		{	
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.TimeSlash);
		}
	}

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick() { }

	public override void Exit() { }
}

