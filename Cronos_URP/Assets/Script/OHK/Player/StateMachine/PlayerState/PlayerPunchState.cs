using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Player의 펀치상태를 정의하는 클래스
/// (현재 사용하지 않음)
/// 
/// ohk    v1
/// </summary>
public class PlayerPunchState : PlayerBaseState
{
	private readonly int AttackHash = Animator.StringToHash("Hook");
	private const float CrossFadeDuration = 0.1f;

	public PlayerPunchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		_stateMachine.Animator.Rebind();
		_stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
	}

	public override void Tick()
	{
		// 현재 애니메이션 정보를 받아온다
		AnimatorStateInfo stateInfo = _stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

		// 애니메이션이 끝났다면
		if (stateInfo.IsName("Hook") && stateInfo.normalizedTime >= 0.3)
		{
			_stateMachine.HitStop.isHit = true;
			_stateMachine.HitStop.StartCoroutine(_stateMachine.HitStop.HitStopCoroutine());
		}

		// 애니메이션이 끝났다면
		if (stateInfo.IsName("Hook") && stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
		{
			_stateMachine.SwitchState(new PlayerIdleState(_stateMachine));
		}
	}

	public override void FixedTick() { }
	public override void LateTick() { }
	public override void Exit() { }
}
