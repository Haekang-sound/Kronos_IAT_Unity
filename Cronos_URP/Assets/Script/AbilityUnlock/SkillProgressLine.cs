using UnityEngine;

/// <summary>
/// ��� ���� ���¸� ǥ���ϴ� UI�� �����մϴ�. 
/// ����� ���¿� ���� ���� ���¸� ������Ʈ�մϴ�.
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
    /// ��� ���°� ����� ������ ���� ���¸� ������Ʈ�մϴ�. ����� Ȱ��ȭ�Ǹ� 100%, �׷��� ������ 0%�� �����˴ϴ�.
    /// </summary>
    public void OnSkillAmountChanged()
    {
        if (node.CurrentState == AbilityNode.State.Activate)
        {
            UpdatePercentage(1f); // ����� Ȱ��ȭ�� ���, ���� ���¸� 100%�� ����
        }
        else
        {
            UpdatePercentage(0f); // ����� ��Ȱ��ȭ�� ���, ���� ���¸� 0%�� ����
        }
    }
}
