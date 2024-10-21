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

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		stateMachine.GroundChecker.ToggleChecker = false;
		
	}
	public override void Tick()
	{
			Vector3 temp = stateMachine.AutoTargetting.GetTarget().position;
			temp.y -= stateMachine.AutoTargetting.GetTarget().localPosition.y;
			stateMachine.transform.position = temp - stateMachine.transform.forward;
		



	}
	public override void FixedTick()
	{
// 		Vector3 gravity = Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
// 
// 		Vector3 temp = stateMachine.AutoTargetting.GetTarget().position - stateMachine.transform.position;
// 
// 
// 		if (temp.magnitude < 1f)
// 		{
// 			Debug.Log("°¡±õ³× ¸Ø­Ÿ~");
// 		}
// 		else if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
// 		{
// 			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.fixedDeltaTime) * stateMachine.MoveForce + gravity;
// 		}
// 		else if (stateMachine.Animator.deltaPosition != null)
// 		{
// 			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.fixedDeltaTime) + gravity;
// 		}
		Float();
	}
	public override void LateTick() { }
	public override void Exit()
	{

		stateMachine.GroundChecker.ToggleChecker = true;
	}



}

