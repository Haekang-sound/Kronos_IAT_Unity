using Cinemachine;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
	//private readonly int JumpHash = Animator.StringToHash("Jump");
	private readonly int JumpHash = Animator.StringToHash("Parry");
	private const float CrossFadeDuration = 0.3f;

	public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Velocity = new Vector3(stateMachine.Velocity.x, stateMachine.Player.jumpForce, stateMachine.Velocity.z);
		stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

		CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (cinemachineBrain != null)
		{
			cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
		}


	}
	public override void Tick()
	{


		if(stateMachine.Velocity.y <= 0f) 
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine));
		}
	}
	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (cinemachineBrain != null)
		{
			cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
		}

	}
}
