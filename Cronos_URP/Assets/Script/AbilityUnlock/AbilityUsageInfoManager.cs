using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUsageInfoManager : MonoBehaviour
{
    private AbilityUsageInfo abilityUsageInfo;

    public void UseEnforcedCombo()
    {
        abilityUsageInfo.EnforcedCombo = true;
    }

    public void UseDodgeAttack()
    {
        abilityUsageInfo.DodgeAttack = true;
    }

    public void UseAttackBan()
    {
        abilityUsageInfo.ComAttackBan = true;
    }

    public void UseNorAttackBan()
    {
        abilityUsageInfo.NorAttackBan = true;
    }

    public void UseRigidImmunity()
    {
        abilityUsageInfo.RigidImmunity = true;
    }

    public void UseNorAttackVariation()
    {
        abilityUsageInfo.NorAttackVariation = true;
    }

    public void UseComAttackVariation()
    {
        abilityUsageInfo.ComAttackVariation = true;
    }

    public void UseComSAttackUpgrade()
    {
        abilityUsageInfo.comSAttackUpgrade = true;
    }

    public void UseNorSAttackUpgrade()
    {
        abilityUsageInfo.norSAttackUpgrade = true;
    }

    public void UseAttackStack()
    {
        abilityUsageInfo.attackStack = true;
    }

}
