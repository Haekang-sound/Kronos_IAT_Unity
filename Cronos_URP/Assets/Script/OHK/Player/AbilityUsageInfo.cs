public struct AbilityUsageInfo
{
	/// 오해강
	//콤보 강화
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool("EnforcedCombo"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("EnforcedCombo", value); }
	}

	//회피 공격
	public bool DodgeAttack
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("DodgeAttack"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("DodgeAttack", value); }
	}

	//Com_Attack 사용 불가
	public bool ComAttackBan
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("ComAttackBan"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("ComAttackBan", value); }
	}
	//Nor_Attack 사용 불가
	public bool NorAttackBan
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("NorAttackBan"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("NorAttackBan", value); }
	}


	//경직 면역
	public bool RigidImmunity
	{
		get { return PlayerStateMachine.GetInstance().Player.RigidImmunity; }
		set { PlayerStateMachine.GetInstance().Player.RigidImmunity = value; }
	}

	/// 민동휘
	//Com_S_Attack 강 공격 강화

	/// 김주영
	//Nor_S_Attack 강 공격 강화
	//공격 스택
}
