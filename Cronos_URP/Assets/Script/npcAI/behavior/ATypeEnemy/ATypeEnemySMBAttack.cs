using UnityEngine;

/// <summary>
/// ATypeEnem�� ���� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class ATypeEnemySMBAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_monoBehaviour.ChangeDebugText("ATTACK");
        _monoBehaviour.inAttack = true;
        _monoBehaviour.UseBulletTimeScale();
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.EndAttack();
        _monoBehaviour.ResetAiming();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.inAttack = false;
    }
}
