using UnityEngine;

/// <summary>
/// ������ ȸ�� �ִϸ��̼� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BossSMBAvoid : SceneLinkedSMB<BossBehavior>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }

}
