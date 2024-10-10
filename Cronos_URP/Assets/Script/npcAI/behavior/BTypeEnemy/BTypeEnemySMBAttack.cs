using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTypeEnemySMBAttack : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("ATTACK");
        _monoBehaviour.aimEnd.SetActive(true);

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.LookAtTarget();

        // DOWN - ���� ������ �÷��̾��� Ư�� �ִϸ��̼� �� ��
        /// TODO = ���� �߰� ���
        if (false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // IDLE - Ÿ���� ã�� �� ���� ��
        _monoBehaviour.FindTarget();
        if (_monoBehaviour.CurrentTarget == null)
        {
            _monoBehaviour.TriggerIdle();
        }

        // RELOAD - �ִϸ��̼��� ���� ���� ��

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.aimEnd.SetActive(false);
    }
}
