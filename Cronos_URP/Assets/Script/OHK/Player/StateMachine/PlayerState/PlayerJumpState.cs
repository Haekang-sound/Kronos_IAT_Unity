using Cinemachine;
using UnityEngine;

/// <summary>
/// Player의 점프상태를 정의하는 클래스
/// (현재 사용 하지 않음)
/// 
/// ohk    v1
/// </summary>
public class PlayerJumpState : PlayerBaseState
{
	private readonly int JumpHash = Animator.StringToHash("Parry");
	private const float CrossFadeDuration = 0.3f;

	public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		_stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

		CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (cinemachineBrain != null)
		{
			cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
		}
	}

	public override void Tick()
	{
		if(_stateMachine.velocity.y <= 0f) 
		{
			_stateMachine.SwitchState(new PlayerFallState(_stateMachine));
		}
	}

	public override void FixedTick(){}
	public override void LateTick(){}

	public override void Exit()
	{
		CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (cinemachineBrain != null)
		{
			cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
		}
	}
}
