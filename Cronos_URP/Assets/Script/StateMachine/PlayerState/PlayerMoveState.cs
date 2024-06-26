using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

// �÷��̾� �⺻���¸� ��ӹ��� movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
	private const float AnimationDampTime = 0.1f;
	private const float CrossFadeDuration = 0.1f;


	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
		stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state�� update�� �� �� ����
	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
		}

		if(Input.GetKeyDown(KeyCode.Tab))
		{
			stateMachine.SwitchState(new PlayerLockOnState(stateMachine));
		}

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent�������� ���� ������� �ʴٸ�
		if (!IsGrounded())
		{
			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
		}

		float moveSpeed = 0.5f;

		if (Input.GetButton("Run"))
		{
			moveSpeed *= 2;
		}
		else { moveSpeed = 0.5f; }

		stateMachine.Player.SetSpeed(moveSpeed);

		// �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
		stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);

		CalculateMoveDirection();   // ������ ����ϰ�

	}
	public override void FixedTick()
	{
		FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
		Move();                     // �̵��Ѵ�.	
	}

	public override void LateTick() {}

	public override void Exit()
	{
		// ���¸� Ż���Ҷ��� jump�� ���� Action�� �������ش�.
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

	private void SwitchToParryState()
	{
		Debug.Log("������");
		stateMachine.SwitchState(new PlayerParryState(stateMachine));
	}

	private void SwitchToLAttackState()
	{
			stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}
	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}
}




