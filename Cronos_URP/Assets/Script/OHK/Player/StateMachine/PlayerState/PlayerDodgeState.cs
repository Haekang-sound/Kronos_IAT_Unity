using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player�� ȸ�ǻ��¸� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		_stateMachine.InputReader.onLAttackStart += Attack;

		// ȸ�ǽ� �̵����̶��
		// �̵��������� �÷��̾ ȸ����Ų��
		if (_stateMachine.velocity.magnitude != 0f)
		{
			_stateMachine.transform.rotation = Quaternion.LookRotation(_stateMachine.velocity);
		}
	}

	public override void Tick()
	{
		// �̵��� ������ �ݿ��Ѵ�.
		if (Time.deltaTime == 0f) return;
		else if(_stateMachine.velocity.magnitude != 0f)
		{
			_stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(_stateMachine.Animator.deltaPosition / Time.deltaTime) * _stateMachine.MoveForce;
		}
		else
		{
			_stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(_stateMachine.Animator.deltaPosition / Time.deltaTime);
		}
		
	}

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick() { }

	public override void Exit()
	{
		_stateMachine.InputReader.onLAttackStart -= Attack;
	}

	private void Attack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.Attack, true);
	}
}
