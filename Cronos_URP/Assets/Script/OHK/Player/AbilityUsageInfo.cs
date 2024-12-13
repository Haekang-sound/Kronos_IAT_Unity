/// <summary>
///  능력의 사용여부를 
///  결정하는 함수들을 모아놓은 클래스
///  
/// ohk    v1 
/// </summary>
public struct AbilityUsageInfo
{
	/// 오해강
	//콤보 강화
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.EnforcedCombo); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.EnforcedCombo, value); }
	}

	//회피 공격
	public bool DodgeAttack
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.DodgeAttack); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.DodgeAttack, value); }
	}

	//경직 면역
	public bool RigidImmunity
	{
		get { return PlayerStateMachine.GetInstance().Player.IsRigidImmunity; }
		set { PlayerStateMachine.GetInstance().Player.IsRigidImmunity = value; }
	}

	// Nor_Attack 변환
	public bool NorAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.NorAttackVariation); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.NorAttackVariation, value); }
	}

	// Com_Attack 변환
	public bool ComAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.ComAttackVariation); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.ComAttackVariation, value); }
	}

	/// 민동휘
	//Com_S_Attack 강 공격 강화
	public bool comSAttackUpgrade;

	/// 김주영
    //Nor_S_Attack 강 공격 강화
    public bool norSAttackUpgrade;
    //공격 스택
    public bool attackStack;
}
