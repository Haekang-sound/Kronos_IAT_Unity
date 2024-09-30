using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUsageInfoManager : MonoBehaviour
{
    private AbilityUsageInfo abilityUsageInfo;

	//콤보 강화
	public void UseEnforcedCombo()
    {
        abilityUsageInfo.EnforcedCombo = true;
    }

	//회피 공격 == 돌진 공격
	public void UseDodgeAttack()
    {
        abilityUsageInfo.DodgeAttack = true;
    }

	//심판의 일섬 - Com_S_Attack_Aura
	public void Com_S_Attack_Aura()
	{
		EffectManager.Instance.isSwordWave = true;
	}

	//심판의 일격 - Com_S_Attack_Ground
	public void Com_S_Attack_Ground()
	{
		EffectManager.Instance.isGroundEnforced = true;
	}

	//	CP 폭발 - Mov_TimeBomb
	public void Mov_TimeBomb()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("CPBoomb", true);
	}
	//	시간 정지 - Mov_TimeStop
	// 격렬한 일격 - Nor_AbilityAttack



	//경직 면역
	public void UseRigidImmunity()
    {
        abilityUsageInfo.RigidImmunity = true;
    }

	// Nor_Attack 변환
	public void UseNorAttackVariation()
    {
        abilityUsageInfo.NorAttackVariation = true;
    }

	// Com_Attack 변환
	public void UseComAttackVariation()
    {
        abilityUsageInfo.ComAttackVariation = true;
    }

	//Com_S_Attack 강 공격 강화
	public void UseComSAttackUpgrade()
    {
        abilityUsageInfo.comSAttackUpgrade = true;
    }

	//Nor_S_Attack 강 공격 강화
	public void UseNorSAttackUpgrade()
    {
        abilityUsageInfo.norSAttackUpgrade = true;
    }

}
