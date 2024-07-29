using UnityEditor;
using UnityEngine;
public class PlayerLockOnState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
	private const float AnimationDampTime = 0.1f;
	private const float CrossFadeDuration = 0.1f;

	public PlayerLockOnState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		Debug.Log("enter");

		stateMachine.Player.IsLockOn = true;

		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		// 자동조준을 해제하고 
		stateMachine.AutoTargetting.LockOff();
		// 대상을 찾고
		stateMachine.AutoTargetting.FindTarget();
		// lockOn한다.
		stateMachine.AutoTargetting.LockOn();

		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // 스테이트에 돌입할때 input에 맞는 함수를 넣어준다
		stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		stateMachine.InputReader.onSwitchingStart += Deceleration;

	}
	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			stateMachine.AutoTargetting.SwitchTarget();
			//stateMachine.AutoTargetting.LockOff();
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine)); // 상태를 생성해서 접근한다.
		}

		// 플레이어의 cp 를 이동속도에 반영한다.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent기준으로 땅에 닿아있지 않다면
		//if (!IsGrounded())
		//{
			//stateMachine.SwitchState(new PlayerFallState(stateMachine)); // 상태를 생성해서 접근한다.
		//}
		 
		float moveSpeed = 0.5f;

		stateMachine.Player.SetSpeed(moveSpeed);

		// 애니메이터 movespeed의 파라메터의 값을 정한다.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
		stateMachine.AutoTargetting.LockOn();


		CalculateMoveDirection();   // 방향을 계산하고
	}
	public override void FixedTick()
	{
		Move();                     // 이동한다.	
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		stateMachine.AutoTargetting.LockOff();
		stateMachine.Player.IsLockOn = false;

		stateMachine.InputReader.onJumpPerformed -= SwitchToParryState;
		stateMachine.InputReader.onLAttackStart -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
		stateMachine.InputReader.onSwitchingStart -= Deceleration;
	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 10)
		{
			Debug.Log("몬스터들이 느려진다");
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	// 점프상태로 바꾸는 함수
	private void SwitchToJumpState()
	{
		stateMachine.SwitchState(new PlayerJumpState(stateMachine));
	}
	private void SwitchToParryState()
	{
		Debug.Log("구른다");
		stateMachine.SwitchState(new PlayerParryState(stateMachine));
	}

	private void SwitchToLAttackState()
	{
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}
	private void SwitchToRAttackState()
	{
		stateMachine.SwitchState(new PlayerPunchState(stateMachine));
	}
	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}
