using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Player의 패리상태를 정의하는 클래스
/// 
/// ohk    v1
/// </summary>
public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	[SerializeField] private float _moveForce;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;

	public override void Enter()
	{
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.ParryAttack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);

	}

	public override void Tick() { }

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick() { }
	public override void Exit() { }

}

