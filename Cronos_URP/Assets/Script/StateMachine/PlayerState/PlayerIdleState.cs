using System.Runtime.InteropServices;
using UnityEngine;


// �⺻����
// �ִϸ��̼� : idle
// ��
public class PlayerIdleState : PlayerBaseState
{
	private readonly int idleHash = Animator.StringToHash("Idle");
	private readonly float duration = 0.3f;
	private bool isMove = false;

	public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	
	public override void Enter()
	{
		//stateMachine.InputReader.onLAttackStart += SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
		stateMachine.InputReader.onLockOnStart += LockOn;

		stateMachine.InputReader.onSwitchingStart += Deceleration;

		stateMachine.InputReader.onMove += IsMove;
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateMachine.Animator.SetTrigger("Attack");
			stateMachine.SwitchState(new PlayerAttackState(stateMachine));
		}

		// playerComponent�������� ���� ������� �ʴٸ�
 		if (!IsGrounded())
 		{
 			stateMachine.SwitchState(new PlayerFallState(stateMachine)); // ���¸� �����ؼ� �����Ѵ�.
 		}
		// �����̸� == �̵�Ű�Է��� ������
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			// �̵����·� �ٲ��
			stateMachine.Animator.SetBool("isMove", true);
			stateMachine.SwitchState(new PlayerMoveState(stateMachine));
		}
	}
	public override void FixedTick() {}
	public override void LateTick()	{}
	public override void Exit()
	{
		stateMachine.InputReader.onMove -= IsMove;
		//stateMachine.InputReader.onLAttackStart -= SwitchToLAttackState;
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
		stateMachine.InputReader.onLockOnStart -= LockOn;

		stateMachine.InputReader.onSwitchingStart -= Deceleration;
	}

	private void SwitchToLAttackState()
	{
		stateMachine.Animator.SetTrigger("Attack");
		stateMachine.SwitchState(new PlayerAttackState(stateMachine));
	}

	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 100)
		{
			Debug.Log("���͵��� ��������");
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}

	}

	private void LockOn()
	{
		if (!stateMachine.Player.IsLockOn)
		{
			// ����� ã��
			stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
		}
		else
		{
			stateMachine.AutoTargetting.LockOff();
		}
	}

	private void IsMove()
	{
		isMove = true;
	}

	private void SwitchToMoveState()
	{
		stateMachine.SwitchState(new PlayerMoveState(stateMachine));
	}
}
