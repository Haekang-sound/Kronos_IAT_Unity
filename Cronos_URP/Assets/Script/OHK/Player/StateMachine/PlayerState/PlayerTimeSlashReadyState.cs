using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeSlashReadyState : PlayerBaseState
{
	public PlayerTimeSlashReadyState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	bool timeSlash;
	float currentTime = 0f;
	Ray frontRay;
	public override void Enter()
	{
		// 0) ĳ���Ϳ� ���� ���̿� ��ֹ��� ������ �����.

		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.ResetTrigger("TimeSlash");
		// 1) ����,

		// 2) �ð�����
		BulletTime.Instance.DecelerateSpeed();
		// 3) ����Ÿ���� ����
		Player.Instance.IsLockOn = true;
		timeSlash = true;

	}
	public override void Tick()
	{

		// ī�޶��� ���� �������� ���̸� �߻�
		Vector3 pos = stateMachine.transform.position;
//		pos.y += 1.4f;
		Ray ray = new Ray(pos, stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position-pos);
		RaycastHit hit;
		Vector3 temp = stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - stateMachine.transform.position;
		// ���̸� �߻��ϰ� Ư�� ���̾ ����
		if (Physics.Raycast(ray, out hit, temp.magnitude, Player.Instance.targetLayer))
		{
			Debug.Log(hit.collider.transform.position);
			// Ư�� ���̾ ���� ������Ʈ�� ������ ���
			Player.Instance.IsLockOn = false;
			stateMachine.Animator.SetTrigger("goIdle");
		}
		else
		{

		}
// 		else
// 		{
// 			// �������� ���� ���
// 			Debug.Log("No object detected on the target layer.");
// 		}


		if (!stateMachine.AutoTargetting.isTargetting)
		{
			stateMachine.Animator.SetTrigger("TimeSlash");
		}
	}
	public override void FixedTick()
	{

		Float();
	}
	public override void LateTick() { }

	public override void Exit()
	{
	}



}

