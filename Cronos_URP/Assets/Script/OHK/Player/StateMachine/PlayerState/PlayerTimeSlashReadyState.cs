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
		// 0) 캐릭터와 몬스터 사이에 장애물이 있으면 멈춘다.

		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.ResetTrigger("TimeSlash");
		// 1) 조준,

		// 2) 시간멈춤
		BulletTime.Instance.DecelerateSpeed();
		// 3) 락온타겟이 나옴
		Player.Instance.IsLockOn = true;
		timeSlash = true;

	}
	public override void Tick()
	{

		// 카메라의 정면 방향으로 레이를 발사
		Vector3 pos = stateMachine.transform.position;
//		pos.y += 1.4f;
		Ray ray = new Ray(pos, stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position-pos);
		RaycastHit hit;
		Vector3 temp = stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - stateMachine.transform.position;
		// 레이를 발사하고 특정 레이어만 감지
		if (Physics.Raycast(ray, out hit, temp.magnitude, Player.Instance.targetLayer))
		{
			Debug.Log(hit.collider.transform.position);
			// 특정 레이어에 속한 오브젝트를 감지한 경우
			Player.Instance.IsLockOn = false;
			stateMachine.Animator.SetTrigger("goIdle");
		}
		else
		{

		}
// 		else
// 		{
// 			// 감지되지 않은 경우
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

