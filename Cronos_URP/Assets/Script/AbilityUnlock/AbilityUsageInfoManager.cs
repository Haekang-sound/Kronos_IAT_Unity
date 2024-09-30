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

	//������ �ϼ� - Com_S_Attack_Aura
	public void Com_S_Attack_Aura()
	{
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
		PlayerStateMachine.GetInstance().Animator.SetBool("CPBoomb", true);
	}
	//	�ð� ���� - Mov_TimeStop
	// �ݷ��� �ϰ� - Nor_AbilityAttack



	//���� �鿪
	public void UseRigidImmunity()
    {
        abilityUsageInfo.RigidImmunity = true;
    }

	// Nor_Attack ��ȯ
	public void UseNorAttackVariation()
    {
        abilityUsageInfo.NorAttackVariation = true;
    }

	// Com_Attack ��ȯ
	public void UseComAttackVariation()
    {
        abilityUsageInfo.ComAttackVariation = true;
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
