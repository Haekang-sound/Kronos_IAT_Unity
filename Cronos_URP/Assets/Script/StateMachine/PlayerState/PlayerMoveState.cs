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
		stateMachine.Velocity.y = Physics.gravity.y;
		stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);

		//stateMachine.InputReader.onJumpPerformed += SwitchToJumpState;	// ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
		stateMachine.InputReader.onJumpPerformed += SwitchToParryState; // ������Ʈ�� �����Ҷ� input�� �´� �Լ��� �־��ش�
		stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state�� update�� �� �� ����
	public override void Tick()
	{
		if(Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
        }

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		// playerComponent�������� ���� ������� �ʴٸ�
		if (!stateMachine.Controller.isGrounded)
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

		ApplyGravity();
		CalculateMoveDirection();   // ������ ����ϰ�
		FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
		Move();                     // �̵��Ѵ�.

	}
	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		// ���¸� Ż���Ҷ��� jump�� ���� Action�� �������ش�.
		//stateMachine.InputReader.onJumpPerformed -= SwitchToJumpState;
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




