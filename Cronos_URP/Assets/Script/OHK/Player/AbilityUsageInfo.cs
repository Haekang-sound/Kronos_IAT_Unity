public struct AbilityUsageInfo
{
	/// ���ذ�
	//�޺� ��ȭ
	public bool EnforcedCombo 
	{
		get{ return PlayerStateMachine.GetInstance().Animator.GetBool("EnforcedCombo"); }
		set { PlayerStateMachine.GetInstance().Animator.SetBool("EnforcedCombo", value); }
	}
	//��ġ ���� -> ���߿�

	//ȸ�� ����


	//Com_Attack ��� �Ұ�
	//Nor_Attack ��� �Ұ�
	//���� �鿪

	/// �ε���
	//Com_S_Attack �� ���� ��ȭ

	/// ���ֿ�
	//Nor_S_Attack �� ���� ��ȭ
	//���� ����

}
