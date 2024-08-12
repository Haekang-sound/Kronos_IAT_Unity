using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
	}
	public override void Tick()
	{
		CalculateMoveDirection();   // ������ ����ϰ�
	}
	public override void FixedTick()
	{
        // Implement code that processes and affects root motion
        // �ִϸ����Ϳ��� ��Ʈ����� �޾ƿ´�. 
        Vector3 rootMotion = stateMachine.Animator.deltaPosition;
        rootMotion.y = 0;
        if (IsOnSlope())
        {
            stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(rootMotion) * stateMachine.MoveForce;
            //Debug.Log(stateMachine.Rigidbody.velocity);
        }
        else
        {
            stateMachine.Rigidbody.velocity = rootMotion * stateMachine.MoveForce;

        }
    }
	public override void LateTick() { }
	public override void Exit(){}
}
