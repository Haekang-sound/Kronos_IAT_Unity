using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	//private readonly int JumpHash = Animator.StringToHash("Parry");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private const float CrossFadeDuration = 0.3f;

	Vector3 direction;

	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Animator.SetTrigger(dodgeHash);
		stateMachine.Player._damageable.isInvulnerable = true;


		// Ű�Է��� �ִٸ�
		if(stateMachine.InputReader.moveComposite.magnitude > 0f)
		{
			direction = stateMachine.Velocity.normalized;
		}
		else // ���ٸ�
		{
			// ī�޶��� ���溤��
			Vector3 temp = Camera.main.transform.forward;
			temp .y = 0f;
			direction = temp.normalized;
		}

	}
	public override void Tick()
	{
		// ȸ�ǹ�����
		// 1. Ű�Է�
		// 2. ī�޶� front
	
		//CalculateMoveDirection();
	}
	public override void FixedTick()
	{
		//FaceMoveDirection();
		stateMachine.Rigidbody.rotation = Quaternion.LookRotation(direction);
		stateMachine.Rigidbody.AddForce(direction * 15f);
	}
	public override void LateTick()
	{
	}

	public override void Exit()
	{
		stateMachine.Player._damageable.isInvulnerable = false;
	}
}
	