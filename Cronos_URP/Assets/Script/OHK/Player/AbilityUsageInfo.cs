/// <summary>
///  �ɷ��� ��뿩�θ� 
///  �����ϴ� �Լ����� ��Ƴ��� Ŭ����
///  
/// ohk    v1 
/// </summary>
public struct AbilityUsageInfo
{
	/// ���ذ�
	//�޺� ��ȭ
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.EnforcedCombo); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.EnforcedCombo, value); }
	}

	//ȸ�� ����
	public bool DodgeAttack
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.DodgeAttack); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.DodgeAttack, value); }
	}

	//���� �鿪
	public bool RigidImmunity
	{
		get { return PlayerStateMachine.GetInstance().Player.IsRigidImmunity; }
		set { PlayerStateMachine.GetInstance().Player.IsRigidImmunity = value; }
	}

	// Nor_Attack ��ȯ
	public bool NorAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.NorAttackVariation); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.NorAttackVariation, value); }
	}

	// Com_Attack ��ȯ
	public bool ComAttackVariation
	{
		get { return PlayerStateMachine.GetInstance().Animator.GetBool(PlayerHashSet.Instance.ComAttackVariation); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.ComAttackVariation, value); }
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
