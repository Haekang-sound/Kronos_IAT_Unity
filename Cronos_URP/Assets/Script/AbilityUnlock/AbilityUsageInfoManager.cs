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

	//심판의 칼날
	public void Com_S_Attack_Aura()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isFlashSlash", true) ;
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
		PlayerStateMachine.GetInstance().Animator.SetBool("isCPBoomb", true);
	}
	//	시간 정지 - Mov_TimeStop
	public void Mov_TimeStop()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isTimeStop", true);
	}
	// 격렬한 강타 - Com_AbilityAttack
	public void Com_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("ComAttackVariation", true);
	}
	// 격렬한 일격 - Nor_AbilityAttack
	public void Nor_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("NorAttackVariation", true);
	}

	// 운명의 굴레 - 패리어택
	public void ParryAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isParry", true);
	}

	// 파괴의 도약 - 
	public void RushAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool("isRushAttack", true);
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
