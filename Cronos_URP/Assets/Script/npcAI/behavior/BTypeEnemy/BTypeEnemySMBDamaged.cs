using UnityEngine;

/// <summary>
/// BTypeEnem�� �ǰ� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BTypeEnemySMBDamaged : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DAMAGED");
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
    }
}