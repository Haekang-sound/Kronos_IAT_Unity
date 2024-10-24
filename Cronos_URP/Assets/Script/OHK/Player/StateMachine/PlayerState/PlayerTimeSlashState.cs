using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeSlashState : PlayerBaseState
{
	public PlayerTimeSlashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] float moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	private bool isArrive;

	Vector3 TargetPosition;
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		stateMachine.GroundChecker.ToggleChecker = false;

		// 도착해야할 위치
		TargetPosition = stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - stateMachine.transform.forward * 1f;

	}

	public override void Tick()
	{
		// 타겟과 캐릭터사이의 거리가 1보다 크다면 타겟쪽으로 다가간다.
		if ((TargetPosition - stateMachine.transform.position).magnitude > 1f)
		{
			stateMachine.GetComponent<Collider>().isTrigger = true;
			if (Mathf.Sqrt((TargetPosition - stateMachine.transform.position).magnitude) < 40f)
			{
				stateMachine.Rigidbody.velocity += (TargetPosition - stateMachine.transform.position).normalized
			* (TargetPosition - stateMachine.transform.position).magnitude * (TargetPosition - stateMachine.transform.position).magnitude;
			}
			else
			{
				stateMachine.Rigidbody.velocity += (TargetPosition - stateMachine.transform.position).normalized
			* 40f;
			}


		}
		else // 도착하면 멈춘다.
		{
			stateMachine.GetComponent<Collider>().isTrigger = false;
			stateMachine.Rigidbody.velocity = Vector3.zero;
		}


	}

	public override void FixedTick()
	{
		Float();
	}
	public override void LateTick() { }
	public override void Exit()
	{

		stateMachine.GroundChecker.ToggleChecker = true;
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

