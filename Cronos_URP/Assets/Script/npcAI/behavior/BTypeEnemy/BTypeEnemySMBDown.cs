using UnityEngine;

/// <summary>
/// BTypeEnem�� ����ȭ ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>

public class BTypeEnemySMBDown : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DOWN");
    }
}