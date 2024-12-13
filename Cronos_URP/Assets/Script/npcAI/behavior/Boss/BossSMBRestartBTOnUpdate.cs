using UnityEngine;

/// <summary>
/// ������ BehaviourTree Runner�� ������ ��Ȱ��ȭ �ϴ� Ŭ���� �Դϴ�.
/// </summary>
public class BossSMBRestartBTOnUpdate : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(true);
    }
}
