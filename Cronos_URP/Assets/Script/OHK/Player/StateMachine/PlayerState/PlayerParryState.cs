using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	Vector3 totalMove;
	[SerializeField] float moveForce;
	bool attackBool = false;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.ParryAttack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);

	}
	public override void Tick()
	{
	}
	public override void FixedTick()
	{
		Float();
	}
	public override void LateTick() { }

	public override void Exit()
	{
		stateMachine.InputReader.onLAttackStart -= Attack;
	}
	private void Attack()
	{
		
		//stateMachine.Animator.SetTrigger("ParryAttack");
	}


}

