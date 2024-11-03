using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		PlayerStateMachine.GetInstance().Animator.SetBool("isFlashSlash", true) ;
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
		PlayerStateMachine.GetInstance().Animator.SetBool("isCPBoomb", true);
	}
	//	�ð� ���� - Mov_TimeStop
	public void Mov_TimeStop()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isTimeStop", true);
	}
	// �ݷ��� ��Ÿ - Com_AbilityAttack
	public void Com_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("ComAttackVariation", true);
	}
	// �ݷ��� �ϰ� - Nor_AbilityAttack
	public void Nor_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("NorAttackVariation", true);
	}

	// ����� ���� - �и�����
	public void ParryAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isParry", true);
	}

	// �ı��� ���� - 
	public void RushAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isRushAttack", true);
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
