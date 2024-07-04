using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBAttack : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("ATTACK");
        _monoBehaviour.ResetTriggerDown();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // DOWN - ���� ������ �÷��̾��� Ư�� �ִϸ��̼� �� ��
        /// TODO = ���� �߰� ���
        if (false)
        {
            //_monoBehaviour.TriggerDown();
        }

        // STRAFE - �ִϸ��̼��� ���� ���� ��
    }

}
