using UnityEngine;

/// <summary>
/// ATypeEnem�� ���� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class ATypeEnemySMBStrongAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("STRONG ATTACK");
        _monoBehaviour.inAttack = true;

        _monoBehaviour.UseBulletTimeScale();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.LookAtTarget();

        // Down - �÷��̾ ���� �丵�� ���� ���� ��
        /// TODO: ���� ���� ���
        if (false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // Strafe - �÷��̾ ���� �и��� ���� ���� ��(�ִϸ��̼��� ��� ���� ��)
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