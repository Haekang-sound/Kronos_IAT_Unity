using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
	private bool ismove = false;
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		CalculateMoveDirection();   // ������ ����ϰ�
		if (stateMachine.Velocity.magnitude != 0f)
		{
			ismove = true;
		}
	}
	public override void FixedTick()
	{
		stateMachine.Rigidbody.AddForce(stateMachine.transform.forward * stateMachine.MoveForce * stateMachine.Animator.speed * Time.fixedDeltaTime, ForceMode.Impulse);
	}
	public override void LateTick() { }
	public override void Exit(){}
}
