using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{
    public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
        stateMachine.AutoTargetting.Target = null;
        stateMachine.Rigidbody.velocity = Vector3.zero;
		Player.Instance._defnsible.isDefending = true;
		Player.Instance._defnsible.onDefensFalse += DefenceFalse;
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
		Player.Instance._defnsible.isDefending = false;
		Player.Instance._defnsible.onDefensFalse -= DefenceFalse;
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
