public struct AbilityUsageInfo
{
	/// ���ذ�
	//�޺� ��ȭ
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool("EnforcedCombo"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("EnforcedCombo", value); }
	}

	//ȸ�� ����
	public bool DodgeAttack
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("DodgeAttack"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("DodgeAttack", value); }
	}

	//Com_Attack ��� �Ұ�
	public bool ComAttackBan
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("ComAttackBan"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("ComAttackBan", value); }
	}
	//Nor_Attack ��� �Ұ�
	public bool NorAttackBan
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("NorAttackBan"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("NorAttackBan", value); }
	}


	//���� �鿪
	public bool RigidImmunity
	{
		get { return PlayerStateMachine.GetInstance().Player.RigidImmunity; }
		set { PlayerStateMachine.GetInstance().Player.RigidImmunity = value; }
	}

	// Nor_Attack ��ȯ
	public bool NorAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("NorAttackVariation"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("NorAttackVariation", value); }
	}

	// Com_Attack ��ȯ
	public bool ComAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool("ComAttackVariation"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("ComAttackVariation", value); }
	}

	/// �ε���
	//Com_S_Attack �� ���� ��ȭ
	public bool comSAttackUpgrade;

    /// ���ֿ�
    //Nor_S_Attack �� ���� ��ȭ
    public bool norSAttackUpgrade;
    //���� ����
    public bool attackStack;
}
