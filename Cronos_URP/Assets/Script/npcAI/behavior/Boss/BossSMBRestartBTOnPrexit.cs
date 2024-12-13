using UnityEngine;

/// <summary>
/// ������ BehaviourTree Runner�� ������ Ȱ��ȭ �ϴ� Ŭ���� �Դϴ�.
/// </summary>
public class BossSMBRestartBTOnPrexit : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(true);
    }
}
