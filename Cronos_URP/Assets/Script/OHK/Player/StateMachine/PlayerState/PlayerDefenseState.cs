using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{
    public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
        stateMachine.AutoTargetting.target = null;
        stateMachine.Rigidbody.velocity = Vector3.zero;
		Player.Instance.defnsible.isDefending = true;
		Player.Instance.defnsible.onDefensFalse += DefenceFalse;
		stateMachine.Player.BeginGuard();
        stateMachine.Player.BeginParry();
    }
	public override void Tick()
    {
       if (!stateMachine.InputReader.IsRAttackPressed)
       {
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
		}
    }
	public override void FixedTick()
    {
        Float();
    }
    public override void LateTick(){}
	public override void Exit()
    {
        stateMachine.Player.EndGuard();
		Player.Instance.defnsible.isDefending = false;
		Player.Instance.defnsible.onDefensFalse -= DefenceFalse;
	}
    public void ReleaseGuard()
    {
        stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
	}

	public void DefenceFalse()
	{
		Debug.Log("데미지를 받아야하는");
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
		stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.damagedA);
	}

}
