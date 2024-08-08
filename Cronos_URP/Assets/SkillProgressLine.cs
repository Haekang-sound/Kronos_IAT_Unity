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

        Nodes.button.onClick.AddListener(OnSkillAmountChanged);
    }

    private void OnDestroy()
    {
        Nodes.button.onClick.RemoveListener(OnSkillAmountChanged);
    }

    public void OnSkillAmountChanged()
    {
        float percentageAmount = GetPercentageFromAmount();
        UpdatePercentage(percentageAmount);
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
