using UnityEngine;

/// <summary>
/// ������ �׺�޽ø� ��Ȱ��ȭ�ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BossSMBOffNavmeshOnEnter : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
    }
}
