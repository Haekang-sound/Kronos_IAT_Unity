public struct AbilityUsageInfo
{
	/// 오해강
	//콤보 강화
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool("EnforcedCombo"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("EnforcedCombo", value); }
	}
	//수치 증가 -> 나중에

	//회피 공격


	//Com_Attack 사용 불가
	//Nor_Attack 사용 불가
	//경직 면역

	/// 민동휘
	//Com_S_Attack 강 공격 강화

	/// 김주영
	//Nor_S_Attack 강 공격 강화
	//공격 스택

}
