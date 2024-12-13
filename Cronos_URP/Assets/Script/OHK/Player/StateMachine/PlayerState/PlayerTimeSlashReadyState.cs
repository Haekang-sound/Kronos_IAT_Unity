using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player의 
/// 시간베기 준비상태를 정의하는 클래스
/// (현재 사용하지 않음)
/// 
/// ohk    v1
/// </summary>
public class PlayerTimeSlashReadyState : PlayerBaseState
{
	public PlayerTimeSlashReadyState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		// 0) 캐릭터와 몬스터 사이에 장애물이 있으면 멈춘다.
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.TimeSlash);
		// 1) 조준,

		// 2) 시간멈춤
		BulletTime.Instance.DecelerateSpeed();

		// 3) 락온타겟이 나옴
		Player.Instance.IsLockOn = true;
	}

	public override void Tick()
	{
		// 카메라의 정면 방향으로 레이를 발사
		Vector3 pos = _stateMachine.transform.position;
		Ray ray = new Ray(pos, _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - pos);
		RaycastHit hit;
		Vector3 temp = _stateMachine.autoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - _stateMachine.transform.position;

		// 레이를 발사하고 특정 레이어만 감지
		if (Physics.Raycast(ray, out hit, temp.magnitude, Player.Instance.targetLayer))
		{
			// 특정 레이어에 속한 오브젝트를 감지한 경우
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

