using JetBrains.Annotations;
using System;
using UnityEngine;

public class SkillProgressLine : ProgressBar
{
    //[SerializeField, CanBeNull] public SkillProgressLine parent;
    [SerializeField] public AbilityNode node;
    [SerializeField] public int requiredLevelsAmount;

    private void Awake()
    {
        node.OnUpdated.AddListener(OnSkillAmountChanged);
    }

    public override void Start()
    {
        base.Start();

        OnSkillAmountChanged();
    }

    private void OnDestroy()
    {
        //node.OnUpdated.RemoveListener(OnSkillAmountChanged);
    }

    public void OnSkillAmountChanged()
    {
        if(node.CurrentState == AbilityNode.State.Activate)
        {
            UpdatePercentage(1f);
        }
    }
}
