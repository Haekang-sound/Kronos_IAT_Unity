using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeSlashState : PlayerBaseState
{
	public PlayerTimeSlashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] float moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;

	float currentTime = 0f;
	bool timeSlash;

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		stateMachine.GroundChecker.ToggleChecker = false;
	}
	public override void Tick()
	{
		CalculateMoveDirection();   // 방향을 계산하고

// 		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
// 		if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
// 		{
// 			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce + gravity;
// 		}
// 		else if (stateMachine.Animator.deltaPosition != null)
// 		{
// 			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
// 		}


		if (timeSlash)
		{
			currentTime += Time.unscaledDeltaTime;
			Player.Instance.currentTime = currentTime;
			PlayerStateMachine.GetInstance().Rigidbody.velocity
				= Player.Instance.transform.forward * Player.Instance.TimeSlashCurve.Evaluate(currentTime);

			Debug.Log(Player.Instance.TimeSlashCurve.Evaluate(currentTime));
			Debug.Log(Player.Instance.transform.forward);
			if (currentTime > 1f)
			{
				timeSlash = false;
				currentTime = 0f;
			}
		}
		else
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

		stateMachine.GroundChecker.ToggleChecker = true;
	}



}

