using UnityEngine;

/// <summary>
/// �÷��̾��� �پ��� �ɷ� �� ���� ��ų�� Ȱ��ȭ�ϰ�, 
/// �ִϸ��̼� �� ȿ���� �����Ͽ� ó���մϴ�.
/// </summary>
public class AbilityUsageInfoManager : MonoBehaviour
{
    private AbilityUsageInfo abilityUsageInfo;

	//�޺� ��ȭ
	public void UseEnforcedCombo()
    {
        abilityUsageInfo.EnforcedCombo = true;
    }

	//ȸ�� ���� == ���� ����
	public void UseDodgeAttack()
    {
        abilityUsageInfo.DodgeAttack = true;
    }

	//������ Į��
	public void Com_S_Attack_Aura()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsFlashSlash, true) ;
		EffectManager.Instance.isSwordWave = true;
	}

	//������ �ϰ� - Com_S_Attack_Ground
	public void Com_S_Attack_Ground()
	{
		EffectManager.Instance.isGroundEnforced = true;
	}

	//	CP ���� - Mov_TimeBomb
	public void Mov_TimeBomb()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsCPBoomb, true);
	}
	//	�ð� ���� - Mov_TimeStop
	public void Mov_TimeStop()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsTimeStop, true);
	}
	// �ݷ��� ��Ÿ - Com_AbilityAttack
	public void Com_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.ComAttackVariation, true);
	}
	// �ݷ��� �ϰ� - Nor_AbilityAttack
	public void Nor_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.NorAttackVariation, true);
	}

	// ����� ���� - �и�����
	public void ParryAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsParry, true);
	}

	// �ı��� ���� - 
	public void RushAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsRushAttack, true);
	}

	//Com_S_Attack �� ���� ��ȭ
	public void UseComSAttackUpgrade()
    {
        abilityUsageInfo.comSAttackUpgrade = true;
    }

	//Nor_S_Attack �� ���� ��ȭ
	public void UseNorSAttackUpgrade()
    {
        abilityUsageInfo.norSAttackUpgrade = true;
    }

}
