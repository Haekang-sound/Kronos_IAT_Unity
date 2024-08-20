using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{
    private readonly int guradHash = Animator.StringToHash("isGuard");
    public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
        stateMachine.AutoTargetting.Target = null;
        stateMachine.Rigidbody.velocity = Vector3.zero;

        stateMachine.Player.BeginGuard();
        stateMachine.Player.BeginParry();
        stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;
    }
	public override void Tick()
    {
//         if (!Input.GetKey(KeyCode.Mouse1))
//         {
//             
//         }
    }
	public override void FixedTick(){}
	public override void LateTick(){}
	public override void Exit()
    {
        stateMachine.Player.EndGuard();
        stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
    }
    public void ReleaseGuard()
    {
        stateMachine.Animator.SetBool(guradHash, false);
    }
}
