using UnityEngine;

/// <summary>
/// ATypeEnem�� �ǰ� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class ATypeEnemySMBDamaged : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ResetAiming();
        _monoBehaviour.ChangeDebugText("DAMAGED");
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
    }
}