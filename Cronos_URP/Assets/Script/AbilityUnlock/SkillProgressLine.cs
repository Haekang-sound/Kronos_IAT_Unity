using UnityEngine;

/// <summary>
/// 기술 진행 상태를 표시하는 UI를 관리합니다. 
/// 기술의 상태에 따라 진행 상태를 업데이트합니다.
/// </summary>
public class SkillProgressLine : ProgressBar
{
    [SerializeField] public AbilityNode node;
    [SerializeField] public int requiredLevelsAmount;

    private void Awake()
    {
        node.OnUpdated.AddListener(OnSkillAmountChanged);
        node.OnLoaded.AddListener(OnSkillAmountChanged);
    }

    public override void Start()
    {
        base.Start();

        OnSkillAmountChanged();
    }

    /// <summary>
    /// 기술 상태가 변경될 때마다 진행 상태를 업데이트합니다. 기술이 활성화되면 100%, 그렇지 않으면 0%로 설정됩니다.
    /// </summary>
    public void OnSkillAmountChanged()
    {
        if (node.CurrentState == AbilityNode.State.Activate)
        {
            UpdatePercentage(1f); // 기술이 활성화된 경우, 진행 상태를 100%로 설정
        }
        else
        {
            UpdatePercentage(0f); // 기술이 비활성화된 경우, 진행 상태를 0%로 설정
        }
    }
}
