using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player의 회피상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerDodgeState : PlayerBaseState
{
	public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		_stateMachine.InputReader.onLAttackStart += Attack;

		// 회피시 이동중이라면
		// 이동방향으로 플레이어를 회전시킨다
		if (_stateMachine.velocity.magnitude != 0f)
		{
			_stateMachine.transform.rotation = Quaternion.LookRotation(_stateMachine.velocity);
		}
	}

	public override void Tick()
	{
		// 이동시 경사면을 반영한다.
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
