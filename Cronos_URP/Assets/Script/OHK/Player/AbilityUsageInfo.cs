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

	/// �ε���
	//Com_S_Attack �� ���� ��ȭ

	/// ���ֿ�
	//Nor_S_Attack �� ���� ��ȭ
	//���� ����
}
