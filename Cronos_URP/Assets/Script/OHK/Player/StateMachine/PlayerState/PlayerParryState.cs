using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
	public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int chargeHash = Animator.StringToHash("Charge");
	private readonly int chargeAttackHash = Animator.StringToHash("chargeAttack");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");
	Vector3 totalMove;
	[SerializeField] float moveForce;
	bool attackBool = false;

	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.Animator.ResetTrigger("Attack");
		stateMachine.Animator.ResetTrigger("Rattack");
		stateMachine.Animator.ResetTrigger("ParryAttack");

		stateMachine.InputReader.onLAttackStart += Attack;
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
	}
	private void Attack()
	{
		stateMachine.Animator.SetTrigger("ParryAttack");
	}


}

