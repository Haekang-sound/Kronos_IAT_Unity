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

		stateMachine.Player.IsLockOn = true;

		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		// �ڵ������� �����ϰ� 
		stateMachine.AutoTargetting.LockOff();
		// ����� ã��
		stateMachine.AutoTargetting.FindTarget();
		// lockOn�Ѵ�.
		stateMachine.AutoTargetting.LockOn();

		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
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
			//stateMachine.SwitchState(new PlayerMoveState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
		}

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent�������� ���� ������� �ʴٸ�
		if (!IsGrounded())
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
		}
		 
		float moveSpeed = 0.5f;

		stateMachine.Player.SetSpeed(moveSpeed);

		// �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
		stateMachine.AutoTargetting.LockOn();


		CalculateMoveDirection();   // ������ ����ϰ�
	}
	public override void FixedTick()
	{
		Move();                     // �̵��Ѵ�.	
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
			Debug.Log("���͵��� ��������");
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	// �������·� �ٲٴ� �Լ�
	private void SwitchToJumpState()
	{
		stateMachine.SwitchState(new PlayerJumpState(stateMachine));
	}
	private void SwitchToParryState()
	{
		Debug.Log("������");
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
