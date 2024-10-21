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

