using JetBrains.Annotations;
using System;
using UnityEngine;

public class SkillProgressLine : ProgressBar
{
    //[SerializeField, CanBeNull] public SkillProgressLine parent;
    [SerializeField] public AbilityNode Nodes;
    [SerializeField] public int requiredLevelsAmount;

    public override void Start()
    {
        base.Start();
        OnSkillAmountChanged();

        Nodes.OnUpdated.AddListener(OnSkillAmountChanged);
    }

    private void OnDestroy()
    {
        Nodes.OnUpdated.RemoveListener(OnSkillAmountChanged);
    }

    public void OnSkillAmountChanged()
    {
        float percentageAmount = GetPercentageFromAmount();
        if (percentageAmount > 0f)
        {
            UpdatePercentage(percentageAmount);
        }
    }

    private bool IsUnlocked()
    {
        return Nodes.abilityLevel.IsNextNodeUnlock();
    }

    private float GetPercentageFromAmount()
    {
        if (!IsUnlocked()) return 0f;

        return Nodes.abilityLevel.GetPercentageOFNextNodeUnlock();
    }
}
