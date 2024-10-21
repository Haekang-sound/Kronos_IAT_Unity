using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerTimeSlashReadyState : PlayerBaseState
{
	public PlayerTimeSlashReadyState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	bool timeSlash;
	float currentTime = 0f;
	public override void Enter()
	{

// 		if (timeSlash)
// 		{
// 			currentTime += Time.unscaledDeltaTime;
// 			Player.Instance.currentTime = currentTime;
// 			PlayerStateMachine.GetInstance().Rigidbody.velocity
// 				= Player.Instance.transform.forward * Player.Instance.TimeSlashCurve.Evaluate(currentTime) * 100f;
// 
// 			Debug.Log(Player.Instance.TimeSlashCurve.Evaluate(currentTime));
// 			Debug.Log(Player.Instance.transform.forward);
// 			if (currentTime > 1f)
// 			{
// 				timeSlash = false;
// 				currentTime = 0f;
// 			}
// 		}
// 		else
// 		{
// 			stateMachine.Animator.SetTrigger("TimeSlash");
// 		}

		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.ResetTrigger("TimeSlash");
		// 1) ¡∂¡ÿ,

		// 2) Ω√∞£∏ÿ√„
		BulletTime.Instance.DecelerateSpeed();
		// 3) ∂Ùø¬≈∏∞Ÿ¿Ã ≥™ø»
		Player.Instance.IsLockOn = true;
		timeSlash = true;

	}
	public override void Tick()
	{

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

