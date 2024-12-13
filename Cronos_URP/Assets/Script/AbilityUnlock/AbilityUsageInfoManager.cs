using UnityEngine;

/// <summary>
/// 플레이어의 다양한 능력 및 공격 스킬을 활성화하고, 
/// 애니메이션 및 효과와 연동하여 처리합니다.
/// </summary>
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
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsFlashSlash, true) ;
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
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsCPBoomb, true);
	}
	//	시간 정지 - Mov_TimeStop
	public void Mov_TimeStop()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsTimeStop, true);
	}
	// 격렬한 강타 - Com_AbilityAttack
	public void Com_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.ComAttackVariation, true);
	}
	// 격렬한 일격 - Nor_AbilityAttack
	public void Nor_AbilityAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.NorAttackVariation, true);
	}

	// 운명의 굴레 - 패리어택
	public void ParryAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsParry, true);
	}

	// 파괴의 도약 - 
	public void RushAttack()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsRushAttack, true);
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
