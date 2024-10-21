using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeSlashState : PlayerBaseState
{
	public PlayerTimeSlashState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	bool timeSlash;
	float currentTime = 0f;
	public override void Enter()
	{

		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.ResetTrigger("TimeSlash");
		// 1) Á¶ÁØ,

		// 2) ½Ã°£¸ØÃã
		BulletTime.Instance.DecelerateSpeed();
		// 3) ¶ô¿ÂÅ¸°ÙÀÌ ³ª¿È
		Player.Instance.IsLockOn = true;
		timeSlash = true;

	}
	public override void Tick()
	{
		
		if (stateMachine.AutoTargetting.isTargetting)
		{
			currentTime += Time.deltaTime;
			Player.Instance.currentTime = currentTime;
			PlayerStateMachine.GetInstance().Rigidbody.velocity
				= Player.Instance.transform.forward * Player.Instance.TimeSlashCurve.Evaluate(currentTime) *300000f;

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
	}



}

