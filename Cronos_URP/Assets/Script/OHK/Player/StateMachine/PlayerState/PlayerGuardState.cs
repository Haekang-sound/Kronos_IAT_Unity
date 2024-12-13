using UnityEngine;

/// <summary>
/// Player�� �����¸� �����ϴ� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class PlayerGuardState : PlayerBaseState
{
    public PlayerGuardState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	public override void Enter()
	{
        _stateMachine.autoTargetting.target = null;
        _stateMachine.Rigidbody.velocity = Vector3.zero;
		Player.Instance.defnsible.isDefending = true;
		Player.Instance.defnsible.onDefensFalse += DefenceFalse;
		_stateMachine.Player.BeginGuard();
        _stateMachine.Player.BeginParry();
    }

	public override void Tick()
    {
       if (!_stateMachine.InputReader.IsRAttackPressed)
       {
			_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
		}
    }

	public override void FixedTick()
    {
        Float();
    }

    public override void LateTick(){}

	public override void Exit()
    {
        _stateMachine.Player.EndGuard();
		Player.Instance.defnsible.isDefending = false;
		Player.Instance.defnsible.onDefensFalse -= DefenceFalse;
	}

    public void ReleaseGuard()
    {
        _stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
	}

	public void DefenceFalse()
	{
		Debug.Log("�������� �޾ƾ��ϴ�");
		_stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
		_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.DamagedA);
	}

}
